using System;

namespace T_API.Core.Settings
{
    public class CacheKeys
    {
        public static string DatabaseTables => "_DatabaseTables";
        public static TimeSpan SlidingExpirationCaching => TimeSpan.FromHours(1);
    }
}