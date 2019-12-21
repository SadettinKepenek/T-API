using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T_API.UI.Areas.Admin.Models.User;

namespace T_API.UI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Client")]
    [Area("Admin")]
    public class UserController : Controller
    {
        private IMapper _mapper;

        public UserController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            // TODO User Listeleme
            return Ok();
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
            

            return View();
        }


    }
}