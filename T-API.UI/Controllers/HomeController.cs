using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using T_API.BLL.Abstract;
using T_API.UI.Extensions;
using T_API.UI.Models;
using T_API.UI.Models.Home;

namespace T_API.UI.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class HomeController : Controller
    {
        private IPackageService _packageService;
        private IUserService _userService;
        private IDatabaseService _databaseService;
        public HomeController(IPackageService packageService, IUserService userService, IDatabaseService databaseService)
        {
            _packageService = packageService;
            _userService = userService;
            _databaseService = databaseService;
        }

        public async Task<IActionResult> Index()
        {
            IndexViewModel viewModel=new IndexViewModel();
            viewModel.Packages = await _packageService.Get();
            viewModel.User = await _userService.GetById(HttpContext.GetNameIdentifier());
            viewModel.Databases = await _databaseService.GetByUser(HttpContext.GetNameIdentifier());
            return View(viewModel);
        }
        public async Task<IActionResult> Documentation()
        {
            return View();
        }


    }
}
