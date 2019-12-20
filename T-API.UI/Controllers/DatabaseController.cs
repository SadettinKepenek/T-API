using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using T_API.BLL.Abstract;

namespace T_API.UI.Controllers
{
    public class DatabaseController : Controller
    {
        private IDatabaseService _databaseService;
        private IMapper _mapper;

        public DatabaseController(IDatabaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
        }


        [HttpGet("MyDatabases")]
        public IActionResult MyDatabases()
        {

            return View();
        }
    }
}