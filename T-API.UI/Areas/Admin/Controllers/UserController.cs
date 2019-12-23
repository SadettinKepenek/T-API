using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T_API.BLL.Abstract;
using T_API.Core.DTO.User;
using T_API.Core.Exception;
using T_API.UI.Areas.Admin.Models.User;

namespace T_API.UI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class UserController : Controller
    {
        private IMapper _mapper;
        private IUserService _userService;

        public UserController(IMapper mapper, IUserService userService)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var users = await _userService.GetAll();
                if (users == null)
                {
                    TempData["Message"] = "Kullanıcı bulunamadı";
                    return RedirectToAction("Index", "User",new {Area = "Admin" });
                }          
                return View(users);
            }
            catch (Exception e)
            {
                TempData["Message"] = ExceptionHandler.HandleException(e).Message;
                throw ExceptionHandler.HandleException(e);
            }

        }
        [HttpGet]
        public async Task<IActionResult> CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Message"] = "Lütfen istenilen bilgileri eksiksiz girin";
                    return View(viewModel);
                }
                var mappedData = _mapper.Map<AddUserDto>(viewModel);
                await _userService.CreateUser(mappedData);
                TempData["Message"] = "Kullanıcı başarıyla oluşturuldu";
                return RedirectToAction("Index", "User", new { Area = "Admin" });
            }
            catch (Exception e)
            {
                TempData["Message"] = ExceptionHandler.HandleException(e).Message;
                throw ExceptionHandler.HandleException(e);
            }


        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                if (id != 0)
                {

                    DeleteUserDto deleteUserDto = new DeleteUserDto();
                    deleteUserDto.UserId = id;
                    deleteUserDto.Role = "Client";
                    await _userService.DeleteUser(deleteUserDto);
                    TempData["Message"] = "Kullanıcı başarıyla silindi";
                    return RedirectToAction("Index", "User", new { Area = "Admin" });
                }
                TempData["Message"] = "Kullanıcı silinirken bir hata oluştu";

                return RedirectToAction("Index", "User", new { Area = "Admin" });
            }
            catch (Exception e)
            {
                TempData["Message"] = ExceptionHandler.HandleException(e).Message;
                throw ExceptionHandler.HandleException(e);

            }


        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                if (id != 0)
                {
                    var user = await _userService.GetById(id);
                    var mappedData = _mapper.Map<UpdateUserViewModel>(user);
                    return View(mappedData);
                }
                TempData["Message"] = "Kullanıcının bilgilerine ulaşılamadı";

                return View();
            }
            catch (Exception e)
            {
                TempData["Message"] = ExceptionHandler.HandleException(e).Message;
                throw ExceptionHandler.HandleException(e);
            }

        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserViewModel updateUserViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Message"] = "Lütfen istenilen bilgileri eksiksiz girin";
                    return View(updateUserViewModel);
                }

                var mappedData = _mapper.Map<UpdateUserDto>(updateUserViewModel);
                await _userService.UpdateUser(mappedData);
                TempData["Message"] = "Kullanıcı başarıyla güncellendi";
                return RedirectToAction("Index", "User", new { Area = "Admin" });
            }
            catch (Exception e)
            {

                TempData["Message"] = ExceptionHandler.HandleException(e).Message;
                throw ExceptionHandler.HandleException(e);
            }

        }


    }
}