using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using T_API.BLL.Abstract;
using T_API.Core.DTO.User;
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
        public RealDatabaseController(IRealDbService realDbService, IDatabaseService databaseService, IAuthService authService, IMapper mapper)
        {
            _realDbService = realDbService;
            _databaseService = databaseService;
            _authService = authService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> ShowEndpoints(int? databaseId)
        {
            int userIdentifier = HttpContext.GetNameIdentifier();
            if (databaseId == null)
            {
                var entities = await _databaseService.GetByUser(userIdentifier);
                if (entities == null)
                {
                    return NoContent();
                }

                return Ok(entities);
            }
            else
            {
                var entity = await _databaseService.GetById((int)databaseId);
                if (entity == null)
                {
                    return NoContent();
                }

                if (entity.UserId == userIdentifier)
                    return Ok(entity);
                else
                    return Unauthorized("Ulaşılmak istenilen veritabanı belirtilen kullanıcıya ait değil.");
            }
        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {

            try
            {
                var mapped = _mapper.Map<LoginUserDto>(loginUserDto);
                LoginResponseDto loginResponseDto = await _authService.Login(mapped,true);
                return Ok(loginResponseDto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return Ok();
        }
    }
}