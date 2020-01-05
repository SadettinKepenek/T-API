using System;
using System.Threading.Tasks;

namespace T_API.Core.Settings
{
    public class CacheKeys
    {
        public static string DatabaseTables => "_DatabaseTables";
        public static TimeSpan SlidingExpirationCaching => TimeSpan.FromMinutes(15);

        public static string DatabaseKey()
        {
            return "_Databases";
        }
        public static string DatabaseKeyById(int dbId)
        {
            return $"_Databases_Database_{dbId}";
        }

        public static string DatabaseKeyByUser(string username)
        {
            return $"_Databases_User_{username}";
        }

        public static string DatabaseKeyByUser(int userId)
        {
            return $"_Databases_User_{userId}";
        }

      

    }
}