using System;

namespace T_API.Core.Settings
{
    public static class ConfigurationSettings
    {
        public static string Host4Travel { get; } = "Data Source=94.73.170.2;initial catalog=u8206796_bnbdb;User Id=u8206796_bnbuser;Password=XFvn39D9MCyr76F";

        public static string SecretKey { get; } =
            "THIS IS USED TO SIGN AND VERIFY JWT TOKENS, REPLACE IT WITH YOUR OWN SECRET, IT CAN BE ANY STRING";

        public static DateTime TokenExpirationDate { get; } = DateTime.UtcNow.AddDays(7);

        public static string CrpytoKey { get; } = "12345678901234561234567890123456";
    }
}