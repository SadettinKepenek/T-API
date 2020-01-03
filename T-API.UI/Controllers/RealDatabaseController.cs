using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using T_API.BLL.Abstract;
using T_API.Core.DAL.Concrete;
using T_API.Core.DTO.RealEndPointManager;
using T_API.Core.DTO.Table;
using T_API.Core.DTO.User;
using T_API.Core.Settings;
using T_API.UI.Extensions;

namespace T_API.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RealDatabaseController : ControllerBase
    {
        private IRealDbService _realDbService;
        private IDatabaseService _databaseService;
        private IAuthService _authService;
        private IMapper _mapper;
        private IDataService _dataService;

        public RealDatabaseController(IRealDbService realDbService, IDatabaseService databaseService, IAuthService authService, IMapper mapper, IDataService dataService)
        {
            _realDbService = realDbService;
            _databaseService = databaseService;
            _authService = authService;
            _mapper = mapper;
            _dataService = dataService;
        }



        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {

            try
            {
                var mapped = _mapper.Map<LoginUserDto>(loginUserDto);
                LoginResponseDto loginResponseDto = await _authService.Login(mapped, true);
                return Ok(loginResponseDto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return Ok();
        }

        [HttpGet("Get/{serviceNumber}/{tableName}")]
        public async Task<IActionResult> Get(int serviceNumber,string tableName,[FromQuery] List<DynamicFilter> filters)
        {
            int userId = HttpContext.GetNameIdentifier();
            var db = await _databaseService.GetById(serviceNumber);
            if (db.UserId!=userId)
            {
                return Unauthorized("Kullanıcı ve Database Sahibi Eşleşmedi");
            }

            var dbInfo = _mapper.Map<DbInformation>(db);
            var data = await _dataService.Get(tableName, dbInfo);

            return Ok(data);
        }
        [HttpPost("Add/{serviceNumber}/{tableName}")]
        public async Task<IActionResult> Add(int serviceNumber, string tableName, [FromForm] IFormCollection formCollection)
        {
            int userId = HttpContext.GetNameIdentifier();
            var db = await _databaseService.GetById(serviceNumber);
            if (db.UserId != userId)
            {
                return Unauthorized("Kullanıcı ve Database Sahibi Eşleşmedi");
            }

            var dbInfo = _mapper.Map<DbInformation>(db);
            await _dataService.Add(tableName, dbInfo, formCollection);

            return Ok(formCollection);
        }



    }
}