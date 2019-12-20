using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace T_API.UI.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Area("Api")]
    [Authorize]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Deneme");
        }
    }
}