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
        private ICacheService _cacheService;

        public AccountController(IUserService userService, IMapper mapper, ICacheService cacheService)
        {
            _userService = userService;
            _mapper = mapper;
            _cacheService = cacheService;
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
                int uId = HttpContext.GetNameIdentifier();
                await _userService.ChangePassword(uId, model.OldPassword, model.NewPassword);
                TempData["Message"] = "Success";
                return View();
            }
            catch (Exception e)
            {
                TempData["Message"] = ExceptionHandler.HandleException(e).Message;
                return View();
            }
        }

       
    }
}