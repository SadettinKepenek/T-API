using System;
using System.Collections.Generic;
using System.Linq;
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
            var databases =await _databaseService.GetByUser(username);
            if (databases==null)
            {
                return View(new MyDatabasesViewModel{Databases = new List<ListDatabaseDto>()});
            }
            return View(new MyDatabasesViewModel
            {
                Databases = databases
            });
        }
    }
}