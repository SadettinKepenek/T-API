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
using T_API.Core.Exception;
using T_API.Core.Settings;
using T_API.UI.Extensions;
using T_API.UI.Models.Account;
using T_API.UI.Models.ViewComponents;

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
            try
            {
                var id = HttpContext.GetNameIdentifier();;
                var userProfile =await _userService.GetById(id);
                if (userProfile==null)
                {
                    TempData["Message"] = SystemMessages.NoContentExceptionMessage;
                    return RedirectToAction("Index", "Home");
                }

                var mappedEntity = _mapper.Map<SettingsViewModel>(userProfile);
                return View(mappedEntity);
            }
            catch (Exception e)
            {
                TempData["Message"] = SystemMessages.DuringOperationExceptionMessage;
                return RedirectToAction("Index", "Home");
            }
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
                TempData["Message"] = SystemMessages.SuccessMessage;
                return RedirectToAction("Index", "Account");
            }
            catch (Exception e)
            {
                TempData["Message"] = SystemMessages.DuringOperationExceptionMessage;
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var uId = HttpContext.GetNameIdentifier();
                await _userService.ChangePassword(uId, model.OldPassword, model.NewPassword);
                TempData["Message"] = SystemMessages.SuccessMessage;
                return View();
            }
            catch (Exception e)
            {
                TempData["Message"] = SystemMessages.DuringOperationExceptionMessage;
                return View();
            }
        }

       
    }
}