using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using T_API.BLL.Abstract;
using T_API.Core.DAL.Concrete;
using T_API.Core.DTO.Column;
using T_API.Core.DTO.Database;
using T_API.Core.DTO.ForeignKey;
using T_API.Core.DTO.Index;
using T_API.Core.DTO.Key;
using T_API.Core.DTO.Table;
using T_API.Core.Exception;
using T_API.Core.Settings;
using T_API.UI.Extensions;
using T_API.UI.Models.Database;

namespace T_API.UI.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class DatabaseController : Controller
    {
        private IDatabaseService _databaseService;
        private IMapper _mapper;
        private IRemoteDbService _remoteDbService;
        private IMemoryCache _cache;
        private IPackageService _packageService;
        private ICacheService _cacheService;
        public DatabaseController(IDatabaseService databaseService, IMapper mapper, IRemoteDbService remoteDbService, IMemoryCache cache, IPackageService packageService, ICacheService cacheService)
        {
            _databaseService = databaseService;
            _mapper = mapper;
            _remoteDbService = remoteDbService;

            _cache = cache;
            _packageService = packageService;
            _cacheService = cacheService;
        }


        public async Task<IActionResult> Index()
        {
            var username = HttpContext.User.Identity.Name;
            var databases = await _databaseService.GetByUser(username);
            if (databases == null)
            {
                return View(new MyDatabasesViewModel { Databases = new List<ListDatabaseDto>() });
            }
            return View(new MyDatabasesViewModel
            {
                Databases = databases
            });
        }

        [HttpGet]
        public async Task<IActionResult> ServiceDetail(int serviceNo)
        {
            try
            {

                var db = await _databaseService.GetById(serviceNo);
                if (db.UserId == HttpContext.GetNameIdentifier())
                {
                    return View(new ServiceDetailViewModel
                    {
                        DatabaseDto = db
                    });
                }
                TempData["Message"] = SystemMessages.NoContentExceptionMessage;
                return RedirectToAction("Index", "Database");

            }
            catch (Exception e)
            {
                TempData["Message"] = SystemMessages.NoContentExceptionMessage;
                return RedirectToAction("Index", "Database");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateService()
        {
            try
            {
                CreateServiceViewModel model = new CreateServiceViewModel();
                model.Packages = await _packageService.Get();
                model.Providers = await _remoteDbService.GetAvailableProviders();
                model.UserId = HttpContext.GetNameIdentifier();
                return View(model);
            }
            catch (Exception e)
            {
                TempData["Message"] =
                    e is DatabaseException ? e.Message : SystemMessages.DuringOperationExceptionMessage;
                return RedirectToAction("Index", "Database");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateService(CreateServiceViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.Packages = await _packageService.Get();
                    model.Providers = await _remoteDbService.GetAvailableProviders();
                    return View(model);
                }

                var dto = _mapper.Map<AddDatabaseDto>(model);
                _ = await _databaseService.AddDatabase(dto);
                TempData["Message"] = SystemMessages.SuccessMessage;
                return RedirectToAction("Index", "Database");


            }
            catch (Exception e)
            {
                TempData["Message"] =
                    e is DatabaseException ? e.Message : SystemMessages.DuringOperationExceptionMessage;
                return RedirectToAction("Index", "Database");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditService(int serviceId)
        {
            try
            {
                var database = await _databaseService.GetById(serviceId);
                if (database.UserId != HttpContext.GetNameIdentifier())
                {
                    TempData["Message"] = SystemMessages.UnauthorizedOperationExceptionMessage;
                    return RedirectToAction("Index", "Database");
                }
                EditServiceViewModel model = _mapper.Map<EditServiceViewModel>(database);
                return View(model);
            }
            catch (Exception e)
            {
                TempData["Message"] =
                    e is DatabaseException ? e.Message : SystemMessages.DuringOperationExceptionMessage;
                return RedirectToAction("Index", "Database");
            }
        }




        public async Task<IActionResult> GetDataTypes(string provider)
        {
            try
            {
                var dataTypes = await _databaseService.GetDataTypes(provider: provider);
                if (dataTypes != null && dataTypes.Count != 0)
                {
                    return Ok(dataTypes);
                }
                return BadRequest(SystemMessages.NoContentExceptionMessage);
            }
            catch (Exception e)
            {
                return BadRequest(SystemMessages.DuringOperationExceptionMessage);
            }
        }




        [HttpPost]
        public async Task<IActionResult> AddColumn([FromBody] AddColumnDto model)
        {


            try
            {
                var db = await _databaseService.GetById(model.DatabaseId);
                if (db.UserId != HttpContext.GetNameIdentifier())
                {
                    return Unauthorized(SystemMessages.UnauthorizedOperationExceptionMessage);
                }
                var dbInformation = _mapper.Map<DbInformation>(db);
                await _remoteDbService.CreateColumnOnRemote(model, dbInformation);
                return Ok(SystemMessages.SuccessMessage);
            }
            catch (Exception e)
            {
                if (e is ValidationException)
                    return BadRequest(e.Message);
                if (e is DatabaseException)
                    return BadRequest(e.Message);
                return BadRequest(SystemMessages.DuringOperationExceptionMessage);
            }

        }

        [HttpPost]
        public async Task<IActionResult> UpdateColumn([FromBody] UpdateColumnDto model)
        {

            try
            {
                var db = await _databaseService.GetById(model.DatabaseId);
                if (db.UserId != HttpContext.GetNameIdentifier())
                {
                    return Unauthorized(SystemMessages.UnauthorizedOperationExceptionMessage);
                }
                var dbInformation = _mapper.Map<DbInformation>(db);
                await _remoteDbService.AlterColumnOnRemote(model, dbInformation);
                return Ok(SystemMessages.SuccessMessage);
            }
            catch (Exception e)
            {
                if (e is ValidationException)
                    return BadRequest(e.Message);
                if (e is DatabaseException)
                    return BadRequest(e.Message);
                return BadRequest(SystemMessages.DuringOperationExceptionMessage);

            }

        }
        [HttpDelete]
        public async Task<IActionResult> DeleteColumn([FromBody] DeleteColumnDto model)
        {

            if (model == null)
                return BadRequest("Gönderilen veri boş");
            try
            {
                var db = await _databaseService.GetById(model.DatabaseId);
                if (db.UserId != HttpContext.GetNameIdentifier())
                {
                    return Unauthorized(SystemMessages.UnauthorizedOperationExceptionMessage);
                }
                var dbInformation = _mapper.Map<DbInformation>(db);

                await _remoteDbService.DropColumnOnRemote(model, dbInformation);
                return Ok(SystemMessages.SuccessMessage);
            }
            catch (Exception e)
            {
                if (e is ValidationException)
                    return BadRequest(e.Message);
                if (e is DatabaseException)
                    return BadRequest(e.Message);

                return BadRequest(SystemMessages.DuringOperationExceptionMessage);
            }

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTable([FromBody] DeleteTableDto model)
        {

            if (model == null)
                return BadRequest("Gönderilen veri boş");
            try
            {
                var db = await _databaseService.GetById(model.DatabaseId);
                if (db.UserId != HttpContext.GetNameIdentifier())
                {
                    return Unauthorized(SystemMessages.UnauthorizedOperationExceptionMessage);
                }
                var dbInformation = _mapper.Map<DbInformation>(db);

                await _remoteDbService.DropColumnOnRemote(model, dbInformation);
                return Ok(SystemMessages.SuccessMessage);
            }
            catch (Exception e)
            {
                if (e is ValidationException)
                    return BadRequest(e.Message);
                if (e is DatabaseException)
                    return BadRequest(e.Message);

                return BadRequest(SystemMessages.DuringOperationExceptionMessage);
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddForeignKey([FromBody] AddForeignKeyDto model)
        {
            try
            {
                var db = await _databaseService.GetById(model.DatabaseId);
                if (db.UserId != HttpContext.GetNameIdentifier())
                {
                    return Unauthorized(SystemMessages.UnauthorizedOperationExceptionMessage);
                }
                var dbInformation = _mapper.Map<DbInformation>(db);

                await _remoteDbService.CreateForeignKeyOnRemote(model, dbInformation);
                return Ok(SystemMessages.SuccessMessage);
            }
            catch (Exception e)
            {
                if (e is ValidationException)
                    return BadRequest(e.Message);
                if (e is DatabaseException)
                    return BadRequest(e.Message);
                return BadRequest(SystemMessages.DuringOperationExceptionMessage);
            }

        }
        [HttpPost]
        public async Task<IActionResult> UpdateForeignKey([FromBody] UpdateForeignKeyDto model)
        {

            if (model == null)
                return BadRequest("Gönderilen veri boş");
            try
            {
                var db = await _databaseService.GetById(model.DatabaseId);
                if (db.UserId != HttpContext.GetNameIdentifier())
                {
                    return Unauthorized(SystemMessages.UnauthorizedOperationExceptionMessage);
                }
                var dbInformation = _mapper.Map<DbInformation>(db);
                await _remoteDbService.AlterForeignKeyOnRemote(model, dbInformation);
                return Ok(SystemMessages.SuccessMessage);
            }
            catch (Exception e)
            {
                if (e is ValidationException)
                    return BadRequest(e.Message);
                if (e is DatabaseException)
                    return BadRequest(e.Message);
                return BadRequest(SystemMessages.DuringOperationExceptionMessage);
            }

        }
        [HttpDelete]
        public async Task<IActionResult> DeleteForeignKey([FromBody] DeleteForeignKeyDto model)
        {

            try
            {
                var db = await _databaseService.GetById(model.DatabaseId);
                if (db.UserId != HttpContext.GetNameIdentifier())
                {
                    return Unauthorized(SystemMessages.UnauthorizedOperationExceptionMessage);
                }
                var dbInformation = _mapper.Map<DbInformation>(db);
                await _remoteDbService.DropForeignKeyOnRemote(model, dbInformation);
                return Ok(SystemMessages.SuccessMessage);
            }
            catch (Exception e)
            {
                if (e is ValidationException)
                    return BadRequest(e.Message);
                if (e is DatabaseException)
                    return BadRequest(e.Message);
                return BadRequest(SystemMessages.DuringOperationExceptionMessage);
            }

        }



        [HttpGet]
        public async Task<IActionResult> GetDatabase(int databaseId)
        {
            try
            {
                var userId = HttpContext.GetNameIdentifier();


                await _cacheService.RemoveCache(userId);
                var database = await _databaseService.GetById(databaseId);

                if (database.UserId != userId)
                {
                    return Unauthorized(SystemMessages.UnauthorizedOperationExceptionMessage);
                }

                return Ok(database);
            }
            catch (Exception e)
            {
                if (e is DatabaseException)
                    return BadRequest(e.Message);
                return BadRequest(SystemMessages.DuringOperationExceptionMessage);
            }

        }


        [HttpGet]
        public async Task<IActionResult> GetTable(int databaseId, string tableName, string provider)
        {
            try
            {
                var userId = HttpContext.GetNameIdentifier();
                var db = await _databaseService.GetById(databaseId);
                if (db.UserId != userId)
                {
                    return Unauthorized(SystemMessages.UnauthorizedOperationExceptionMessage);
                }
                var table = await _remoteDbService.GetTable(tableName, db.DatabaseName, provider);
                if (table == null)
                {
                    return BadRequest(SystemMessages.NoContentExceptionMessage);
                }

                return Ok(table);
            }
            catch (Exception e)
            {
                if (e is DatabaseException)
                    return BadRequest(e.Message);
                return BadRequest(SystemMessages.DuringOperationExceptionMessage);
            }

        }


        [HttpGet]
        public async Task<IActionResult> AddTable(int databaseId)
        {
            try
            {
                var db = await _databaseService.GetById(databaseId);

                var userId = HttpContext.GetNameIdentifier();
                if (userId != db.UserId)
                {
                    TempData["Message"] = SystemMessages.UnauthorizedOperationExceptionMessage;
                    return RedirectToAction("Index", "Database");
                }

                AddTableViewModel addTableViewModel = new AddTableViewModel
                {
                    Indices = new List<AddIndexDto>(),
                    Columns = new List<AddColumnDto>(),
                    Keys = new List<AddKeyDto>(),
                    ForeignKeys = new List<AddForeignKeyDto>(),
                    TableName = "",
                    Provider = db.Provider,
                    DatabaseName = db.DatabaseName,
                    DatabaseId = db.DatabaseId
                };
                return View(addTableViewModel);
            }
            catch (Exception e)
            {
                TempData["Message"] =
                    e is DatabaseException ? e.Message : SystemMessages.DuringOperationExceptionMessage;
                return RedirectToAction("Index", "Database");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTable(AddTableViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var db = await _databaseService.GetById(model.DatabaseId);


                var userId = HttpContext.GetNameIdentifier();
                if (userId != db.UserId)
                {
                    TempData["Message"] = SystemMessages.UnauthorizedOperationExceptionMessage;
                    return RedirectToAction("Index", "Database");
                }

                foreach (AddColumnDto addColumnDto in model.Columns)
                {
                    addColumnDto.TableName = model.TableName;
                    addColumnDto.DatabaseId = model.DatabaseId;
                    addColumnDto.Provider = model.Provider;
                }
                var mappedEntity = _mapper.Map<AddTableDto>(model);
                var dbInfo = _mapper.Map<DbInformation>(db);
                await _remoteDbService.CreateTableOnRemote(mappedEntity, dbInfo);

                TempData["Message"] = SystemMessages.SuccessMessage;
                return RedirectToAction("EditService", "Database", new { serviceId = model.DatabaseId });
            }
            catch (Exception e)
            {
                TempData["Message"] =
                    e is DatabaseException ? e.Message : SystemMessages.DuringOperationExceptionMessage;
                return RedirectToAction("Index", "Database");
            }
        }

    }
}