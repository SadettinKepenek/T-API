using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T_API.BLL.Abstract;
using T_API.Core.DTO.Database;
using T_API.DAL.Abstract;
using T_API.UI.Areas.Admin.Models.Database;

namespace T_API.UI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class DatabaseController : Controller
    {
        IDatabaseService _databaseService;

        public DatabaseController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        public async Task<IActionResult> GetAllDatabases()
        {
            var databases = await _databaseService.GetAll();
            if (databases != null)
            {
                return View(databases);
            }
            return RedirectToAction("Index","Home");

        }
        public async Task<IActionResult> UpdateDatabase(int id)
        {
            if (id != 0)
            {
                var database = await _databaseService.GetById(id);
                
                return View(new UpdateDatabaseViewModel { 
                    Username = database.Username,
                    Password = database.Password,
                    UserId = database.UserId,
                    EndDate = database.EndDate,
                    IsActive = database.IsActive,
                    IsApiSupport = database.IsApiSupport,
                    IsStorageSupport = database.IsStorageSupport,
                    Port = database.Port,
                    Server = database.Server,
                    StartDate = database.StartDate,
                    Database = database.Database,
                    Provider = database.Provider,
                    DatabaseId = database.DatabaseId
                });
                
            }
            return View();
        }
        public async Task<IActionResult> UpdateDatabase(UpdateDatabaseViewModel updateDatabaseViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(updateDatabaseViewModel);
            }
            UpdateDatabaseDto updateDatabaseDto = new UpdateDatabaseDto();
            updateDatabaseDto.Database = updateDatabaseViewModel.Database;
            updateDatabaseDto.EndDate = updateDatabaseViewModel.EndDate;
            updateDatabaseDto.StartDate = updateDatabaseViewModel.StartDate;
            updateDatabaseDto.Password = updateDatabaseViewModel.Password;
            updateDatabaseDto.Username = updateDatabaseViewModel.Username;
            updateDatabaseDto.UserId = updateDatabaseViewModel.UserId;
            updateDatabaseDto.Server = updateDatabaseViewModel.Server;
            updateDatabaseDto.Provider = updateDatabaseViewModel.Provider;
            updateDatabaseDto.IsStorageSupport = updateDatabaseViewModel.IsStorageSupport;
            updateDatabaseDto.Port = updateDatabaseViewModel.Port;
            updateDatabaseDto.IsActive = updateDatabaseViewModel.IsActive;
            updateDatabaseDto.IsApiSupport = updateDatabaseViewModel.IsApiSupport;
            await _databaseService.UpdateDatabase(updateDatabaseDto);
            return RedirectToAction("GetAllDatabases","Database");

        }
    }
}