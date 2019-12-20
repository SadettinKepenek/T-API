using System;
using T_API.Core.DAL.Concrete;

namespace T_API.Core.Settings
{
    public static class ConfigurationSettings
    {


        public static DbInformation DbInformation { get; } = new DbInformation
        {
            Provider = "MySql",
            Port = "3306",
            Password = "PJvi50C7GJeo56H",
            Username = "u8206796_userTAP",
            Database = "u8206796_dbTAPI",
            Server = "94.73.170.109"
        };
        public static string SecretKey { get; } =
            "THIS IS USED TO SIGN AND VERIFY JWT TOKENS, REPLACE IT WITH YOUR OWN SECRET, IT CAN BE ANY STRING";

        public static DateTime TokenExpirationDate { get; } = DateTime.UtcNow.AddDays(7);

        public static string CrpytoKey { get; } = "12345678901234561234567890123456";



    }
}