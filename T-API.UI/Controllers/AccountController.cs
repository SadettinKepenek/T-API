using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T_API.BLL.Abstract;
using T_API.Core.DTO.User;
using T_API.UI.Models.Account;

namespace T_API.UI.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class AccountController : Controller
    {
        private IUserService _userService;

        private IMapper _mapper;

        public AccountController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("[controller]/Settings")]
        public async Task<IActionResult> Index()
        {
            var id = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.NameIdentifier)?.Value);
            var userProfile =await _userService.GetById(id);
            if (userProfile==null)
            {
                return RedirectToAction("Index", "Home");
            }

            var mappedEntity = _mapper.Map<SettingsViewModel>(userProfile);
            return View(mappedEntity);
        }

        [HttpPost("[controller]/Settings")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SettingsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                var mappedEntity = _mapper.Map<UpdateUserDto>(viewModel);
                await _userService.UpdateUser(mappedEntity);
                TempData["Message"] = "Güncellendi !";
                return RedirectToAction("Index", "Account");
            }
            catch (Exception e)
            {
                TempData["Message"] = "Güncelleme sırasında hata oluştu";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}