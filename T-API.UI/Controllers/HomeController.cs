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
using T_API.UI.Models;
using T_API.UI.Models.Home;

namespace T_API.UI.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class HomeController : Controller
    {
        private IPackageService _packageService;
        public HomeController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        public async Task<IActionResult> Index()
        {
            IndexViewModel viewModel=new IndexViewModel();
            viewModel.Packages = await _packageService.Get();
            return View(viewModel);
        }
        public async Task<IActionResult> Documentation()
        {
            return View();
        }


    }
}
