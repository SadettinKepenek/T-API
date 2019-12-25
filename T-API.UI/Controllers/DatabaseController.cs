using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T_API.BLL.Abstract;
using T_API.Core.DTO.Database;
using T_API.Core.DTO.Table;
using T_API.Core.Exception;
using T_API.Core.Settings;
using T_API.UI.Models.Database;

namespace T_API.UI.Controllers
{
    [Authorize]
    public class DatabaseController : Controller
    {
        private IDatabaseService _databaseService;
        private IMapper _mapper;
        private IRealDbService _realDbService;
        public DatabaseController(IDatabaseService databaseService, IMapper mapper, IRealDbService realDbService)
        {
            _databaseService = databaseService;
            _mapper = mapper;
            _realDbService = realDbService;
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
            if (serviceNo == 0)
            {
                return RedirectToAction("Index", "Database");
            }

            var db = await _databaseService.GetById(serviceNo);
            if (db == null)
            {
                TempData["Message"] = $"{serviceNo} Numaralı database verilerine ulaşılamadı";
            }
            return View(new ServiceDetailViewModel
            {
                DatabaseDto = db
            });
        }

        [HttpGet]
        public async Task<IActionResult> CreateService()
        {
            CreateServiceViewModel model = new CreateServiceViewModel();
            model.UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateService(CreateServiceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var dto = _mapper.Map<AddDatabaseDto>(model);
                dto.StartDate = DateTime.Now;
                dto.EndDate = DateTime.Now.AddMonths(1);
                dto.Port = ConfigurationSettings.ServerDbInformation.Port;
                dto.Provider = ConfigurationSettings.ServerDbInformation.Provider;
                dto.Server = ConfigurationSettings.ServerDbInformation.Server;
                dto.IsActive = false;
                dto.IsApiSupport = true;
                dto.IsStorageSupport = false;
                _ = await _databaseService.AddDatabase(dto);
                TempData["Message"] = "Database Başarıyla Eklendi";
                return RedirectToAction("Index", "Database");
            }
            catch (Exception e)
            {
                TempData["Message"] = "Database Oluşturulurken hata oluştu";
                return RedirectToAction("Index", "Database");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditService(int serviceId)
        {
            var database = await _databaseService.GetById(serviceId);
            if (database == null)
            {
                TempData["Message"] = "İstenilen database'e ulaşılamadı";
                return RedirectToAction("Index", "Database");
            }

            EditServiceViewModel model = _mapper.Map<EditServiceViewModel>(database);
            //model.UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

            return View(model);
        }




        public async Task<IActionResult> GetDataTypes(string provider)
        {
            var dataTypes = await _databaseService.GetDataTypes(provider: provider);
            if (dataTypes != null && dataTypes.Count != 0)
            {
                return Ok(dataTypes);
            }
            return NoContent();
        }





        [HttpPost("", Name = "AddTable")]
        public async Task<IActionResult> AddTable([FromBody] AddTableDto addTableDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(addTableDto);
            }
            await _realDbService.CreateTableOnRemote(addTableDto);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetDatabase(int databaseId)
        {
            try
            {
                var database = await _databaseService.GetById(databaseId);
                if (database == null)
                {
                    return NoContent();
                }

                var userId = Convert.ToInt32(HttpContext.User.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)
                    ?.Value);
                
                if (database.UserId != userId)
                {
                    return BadRequest("User Id uyuşmuyor");
                }


                //model.UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

                return Ok(database);
            }
            catch (Exception e)
            {
                return BadRequest(e.StackTrace);
            }
          
        }


        // TODO Add Table AJax yapılacak
    }
}