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

            AddUserDto user = new AddUserDto();
            user.Firstname = viewModel.Firstname;
            user.Lastname = viewModel.Lastname;
            user.Email = viewModel.Email;
            user.Password = viewModel.Password;
            user.Username = viewModel.Username;
            user.Role = viewModel.Role;
            user.PhoneNumber = " ";
            await _userService.CreateUser(user);
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
                return View(new UpdateUserViewModel {
                    Username = user.Username,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Email = user.Email,
                    Role = user.Role,
                    UserId = user.UserId,
                    PhoneNumber = user.PhoneNumber,
                    Balance = user.Balance,
                    IsActive = user.IsActive,
                    Password = user.Password
                });
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
            UpdateUserDto updateUserDto = new UpdateUserDto();
            updateUserDto.Firstname = updateUserViewModel.Firstname;
            updateUserDto.Lastname = updateUserViewModel.Lastname;
            updateUserDto.Username = updateUserViewModel.Username;
            updateUserDto.Role = updateUserViewModel.Role;
            updateUserDto.UserId = updateUserViewModel.UserId;
            updateUserDto.IsActive = updateUserViewModel.IsActive;
            updateUserDto.PhoneNumber = updateUserViewModel.PhoneNumber;
            updateUserDto.Password = updateUserViewModel.Password;
            updateUserDto.Balance = updateUserViewModel.Balance;
            updateUserDto.Email = updateUserViewModel.Email;
            await _userService.UpdateUser(updateUserDto);
            return RedirectToAction("Index","User");
        }


    }
}