using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace T_API.UI.Controllers
{
    public class DatabaseController : Controller
    {
        
        [HttpGet("MyDatabases")]
        public IActionResult MyDatabases()
        {

            return View();
        }
    }
}