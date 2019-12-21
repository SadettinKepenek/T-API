using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T_API.BLL.Abstract;
using T_API.Core.DTO.User;
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
            var users = await _userService.GetAll();
            if (users == null)
            {
                return RedirectToAction("Index", "User");
            }
            return View(users);
        }
        [HttpGet]
        public async Task<IActionResult> CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var mappedData = _mapper.Map<AddUserDto>(viewModel);
            await _userService.CreateUser(mappedData);
            return RedirectToAction("Index", "User");
        }

        public async Task<IActionResult> DeleteUser(DeleteUserDto deleteUserDto)
        {
            if (deleteUserDto != null)
            {
                await _userService.DeleteUser(deleteUserDto);
                return RedirectToAction("Index", "User");
            }
            return RedirectToAction("Index", "User");

        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (id != 0)
            {
                var user = await _userService.GetById(id);
                var mappedData = _mapper.Map<UpdateUserViewModel>(user);
               return View(mappedData);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserViewModel updateUserViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(updateUserViewModel);
            }
            var mappedData = _mapper.Map<UpdateUserDto>(updateUserViewModel);
            await _userService.UpdateUser(mappedData);
            return RedirectToAction("Index","User");
        }


    }
}