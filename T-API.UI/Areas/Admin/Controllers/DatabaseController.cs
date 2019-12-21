using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private IMapper _mapper;

        public DatabaseController(IDatabaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
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
                var mappedEntity = _mapper.Map<UpdateDatabaseViewModel>(database);
                return View(database);
                
            }

            TempData["Message"] = "Database parametresi boş gönderilemez";
            return RedirectToAction("Index", "Database",new {Area="Admin"});
        }
        public async Task<IActionResult> UpdateDatabase(UpdateDatabaseViewModel updateDatabaseViewModel)
        {
            if (!ModelState.IsValid)
            { 
                return View(updateDatabaseViewModel);
            }

            var mappedEntity = _mapper.Map<UpdateDatabaseDto>(updateDatabaseViewModel);
            await _databaseService.UpdateDatabase(mappedEntity);
            return RedirectToAction("GetAllDatabases","Database");

        }
    }
}