using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using T_API.BLL.Abstract;
using T_API.Core.DTO.Database;
using T_API.Core.Settings;

namespace T_API.BLL.Concrete
{
    public class CacheManager : ICacheService
    {
        private IUserService _userService;
        private IDatabaseService _databaseService;
        private IMemoryCache _cache;

        public CacheManager(IUserService userService, IDatabaseService databaseService, IMemoryCache cache)
        {
            _userService = userService;
            _databaseService = databaseService;
            _cache = cache;
        }
        public async Task RemoveCache(int userId)
        {
            try
            {
                var user = await _userService.GetById(userId);
                var databases = await _databaseService.GetByUser(userId);

                _cache.Remove(CacheKeys.DatabaseKeyByUser(user.Username));
                _cache.Remove(CacheKeys.DatabaseKeyByUser(user.UserId));
                foreach (var database in databases)
                {
                    _cache.Remove(CacheKeys.DatabaseKeyById(database.DatabaseId));
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}