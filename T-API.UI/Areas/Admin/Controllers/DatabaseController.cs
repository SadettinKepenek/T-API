using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T_API.BLL.Abstract;
using T_API.Core.DTO.Database;
using T_API.Core.Exception;
using T_API.DAL.Abstract;
using T_API.UI.Areas.Admin.Models.Database;

namespace T_API.UI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class DatabaseController : Controller
    {
        IDatabaseService _databaseService;
        private IMapper _mapper;

        public DatabaseController(IDatabaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
        } 
        public async Task<IActionResult> GetAllDatabases()
        {
            try
            {
                var databases = await _databaseService.GetAll();
                if (databases != null)
                {
                    return View(databases);
                }
                TempData["Message"] = "Database bulunamadı";
                return RedirectToAction("Index", "Home", new { Area = "Admin" });
            }
            catch (Exception e)
            {
                TempData["Message"] = ExceptionHandler.HandleException(e).Message;
                throw ExceptionHandler.HandleException(e);
            }


        }
        public async Task<IActionResult> UpdateDatabase(int id)
        {
            try
            {
                if (id != 0)
                {
                    var database = await _databaseService.GetById(id);
                    var mappedEntity = _mapper.Map<UpdateDatabaseViewModel>(database);
                    return View(database);
                }
                TempData["Message"] = "Database parametresi boş gönderilemez";
                return RedirectToAction("Index", "Database", new { Area = "Admin" });
            }
            catch (Exception e)
            {
                TempData["Message"] = ExceptionHandler.HandleException(e).Message;
                throw ExceptionHandler.HandleException(e);
            }

        }
        public async Task<IActionResult> UpdateDatabase(UpdateDatabaseViewModel updateDatabaseViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Message"] = "Lütfen istenilen bilgileri eksiksiz giriniz";
                    return View(updateDatabaseViewModel);
                }

                var mappedEntity = _mapper.Map<UpdateDatabaseDto>(updateDatabaseViewModel);
                await _databaseService.UpdateDatabase(mappedEntity);
                TempData["Message"] = "Database başarıyla güncellendi";
                return RedirectToAction("GetAllDatabases", "Database", new { Area = "Admin" });
            }
            catch (Exception e)
            {
                TempData["Message"] = ExceptionHandler.HandleException(e).Message;
                throw ExceptionHandler.HandleException(e);
            }


        }
        [HttpGet]
        public async Task<IActionResult> CreateDatabase()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateDatabase(CreateDatabaseViewModel createDatabaseViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Message"] = "Lütfen istenilen bilgileri eksiksiz giriniz";
                    return View(createDatabaseViewModel);
                }
                var mappedData = _mapper.Map<AddDatabaseDto>(createDatabaseViewModel);
                await _databaseService.AddDatabase(mappedData);
                TempData["Message"] = "Database başarıyla oluşturuldu";
                return RedirectToAction("GetAllDatabases", "Database", new { Area = "Admin" });
            }
            catch (Exception e)
            {
                TempData["Message"] = ExceptionHandler.HandleException(e).Message;
                throw ExceptionHandler.HandleException(e);
            }
        }

        public async Task<IActionResult> DeleteDatabase(int id)
        {
            try
            {
                if (id != 0)
                {
                    DeleteDatabaseDto deleteDatabaseDto = new DeleteDatabaseDto();
                    deleteDatabaseDto.DatabaseId = id;
                    await _databaseService.DeleteDatabase(deleteDatabaseDto);
                    TempData["Message"] = "Database başarıyla silindi";
                    return RedirectToAction("Index", "User", new { Area = "Admin" });
                }

                TempData["Message"] = "Database silinirken bir hata oluştu";
                return RedirectToAction("Index", "User", new { Area = "Admin" });
            }
            catch (Exception e)
            {
                TempData["Message"] = ExceptionHandler.HandleException(e).Message;
                throw ExceptionHandler.HandleException(e);

            }
        }
    }
}