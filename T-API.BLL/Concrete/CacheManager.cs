using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using T_API.BLL.Abstract;
using T_API.Core.Settings;

namespace T_API.BLL.Concrete
{
    public class CacheManager:ICacheService
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
                _cache.Remove(CacheKeys.DatabaseKeyByUser(user.Username));
                _cache.Remove(CacheKeys.DatabaseKeyByUser(user.UserId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}