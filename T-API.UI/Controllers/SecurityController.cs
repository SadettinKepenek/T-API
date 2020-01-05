using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T_API.BLL.Abstract;
using T_API.BLL.Validators.User;
using T_API.Core.DTO.User;
using T_API.Core.Exception;
using T_API.Core.Settings;
using T_API.UI.Extensions;
using T_API.UI.Models.Security;

namespace T_API.UI.Controllers
{
    public class SecurityController : Controller
    {
        private IAuthService _authService;
        private IMapper _mapper;
        private ICacheService _cacheService;

        public SecurityController(IAuthService authService, IMapper mapper, ICacheService cacheService)
        {
            _authService = authService;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _cacheService.RemoveCache(HttpContext.GetNameIdentifier());
                await _authService.Logout();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                TempData["Message"] = SystemMessages.DuringOperationExceptionMessage;
                return RedirectToAction("Index", "Home");

            }
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                else
                {
                    var mapped = _mapper.Map<LoginUserDto>(model);
                    await _authService.Login(mapped, false);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                TempData["Message"] = SystemMessages.LoginSuccessMessage;
                return RedirectToAction("Index", "Home");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Register()
        {

            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {

            try
            {
                registerViewModel.Role = SystemRoles.Client;
                if (!ModelState.IsValid) return View(registerViewModel);
                var mapped = _mapper.Map<AddUserDto>(registerViewModel);
                await _authService.Register(mapped);
                TempData["Message"] = SystemMessages.SuccessMessage;
                return RedirectToAction("Login", "Security");

            }
            catch (Exception e)
            {
                TempData["Message"] = SystemMessages.DuringOperationExceptionMessage;
                return RedirectToAction("Register", "Security");
            }

        }
    }
}