using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using T_API.BLL.Abstract;

namespace T_API.UI.Areas.Api.Controllers
{
    [Route("apiRoute/[controller]")]
    [ApiController]
    [Area("Api")]
    public class TestsController : ControllerBase
    {
        private IAuthService _authService;
        public TestsController(IAuthService authService)
        {
            _authService = authService;
        }

       
    }
}