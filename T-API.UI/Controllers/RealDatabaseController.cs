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
using Microsoft.Extensions.Caching.Memory;
using T_API.BLL.Abstract;
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
        private IMemoryCache _cache;
        private IEndPointService _endPointService;

        public RealDatabaseController(IRealDbService realDbService, IDatabaseService databaseService, IAuthService authService, IMapper mapper, IMemoryCache cache, IEndPointService endPointService)
        {
            _realDbService = realDbService;
            _databaseService = databaseService;
            _authService = authService;
            _mapper = mapper;
            _cache = cache;
            _endPointService = endPointService;
        }

        [HttpGet]
        public async Task<IActionResult> ShowEndpoints(int databaseId,string tableName)
        {
            int userIdentifier = HttpContext.GetNameIdentifier();
            {
                var entity = await _databaseService.GetById(databaseId);
                if (entity == null)
                {
                    return NoContent();
                }

                if (entity.UserId == userIdentifier)
                {

                    DetailTableDto table;
                    var cacheKey = $"User_{userIdentifier}_Database_{entity.DatabaseId}_Table_{tableName}";
                    // Cache de var ise getiriliyor yoksa db den çekiliyor.
                    var cacheEntry = await _cache.GetOrCreateAsync(cacheKey, async entry =>
                    {
                        entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                        entry.SetPriority(CacheItemPriority.Low);
                        table = await _realDbService.GetTable(tableName,entity.DatabaseName, entity.Provider);
                        return await Task.FromResult(table);
                    });
                    
                    

                    return Ok(_endPointService.GetEndPoints(cacheEntry,entity.UserId,entity.DatabaseId));
                }
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
                LoginResponseDto loginResponseDto = await _authService.Login(mapped, true);
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