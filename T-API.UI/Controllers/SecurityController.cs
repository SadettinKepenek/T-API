using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T_API.BLL.Abstract;
using T_API.BLL.Validators.User;
using T_API.Core.DTO.User;
using T_API.UI.Models.Security;

namespace T_API.UI.Controllers
{
    public class SecurityController : Controller
    {
        private IAuthService _authService;
        private IMapper _mapper;

        public SecurityController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return RedirectToAction("Index", "Home");
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
                if (ModelState.IsValid)
                {
                    var mapped = _mapper.Map<LoginUserDto>(model);
                    await _authService.Login(mapped);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception e)
            {
                
            }
            return View(model);
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
                registerViewModel.Role = "Client";
                if (ModelState.IsValid)
                {
                    var mapped = _mapper.Map<AddUserDto>(registerViewModel);
                    await _authService.Register(mapped);
                    return RedirectToAction("Login", "Security");
                }

                return View(registerViewModel);

            }
            catch (Exception e)
            {
                // ignored
            }

            return View(registerViewModel);
        } 
    }
}