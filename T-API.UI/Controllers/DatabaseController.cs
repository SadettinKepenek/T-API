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
using T_API.UI.Models.Database;

namespace T_API.UI.Controllers
{
    [Authorize]
    public class DatabaseController : Controller
    {
        private IDatabaseService _databaseService;
        private IMapper _mapper;

        public DatabaseController(IDatabaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
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
                var mappedModel = _mapper.Map<AddDatabaseDto>(model);
                _ = await _databaseService.AddDatabase(mappedModel);
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
        public async Task<IActionResult> EditService()
        {
            CreateServiceViewModel model = new CreateServiceViewModel();
            model.UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditService(CreateServiceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var mappedModel = _mapper.Map<UpdateDatabaseDto>(model);
                await _databaseService.UpdateDatabase(mappedModel);
                TempData["Message"] = "Database Başarıyla güncellendi";
                return RedirectToAction("Index", "Database");
            }
            catch (Exception e)
            {
                TempData["Message"] = "Database Güncellenirken hata oluştu";
                return RedirectToAction("Index", "Database");
            }
        }


        [HttpGet("{provider}",Name = "GetDataTypes")]
        public async Task<IActionResult> GetDataTypes(string provider)
        {
            var dataTypes = await _databaseService.GetDataTypes(provider: provider);
            if (dataTypes!=null && dataTypes.Count!=0)
            {
                return Ok(dataTypes);
            }
            return NoContent();
        }

    }
}