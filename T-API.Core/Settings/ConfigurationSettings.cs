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
            Password = "Berkay.0535",
            Username = "byalcin",
            DatabaseName = "maindb",
            Server = "8.208.25.150"
        };

        public static DbInformation ServerDbInformation { get; } = new DbInformation
        {
            Provider = "MySql",
            Port = "3306",
            Password = "Berkay.0535",
            Username = "byalcin",
            DatabaseName = "",
            Server = "8.208.25.150"
        };
        public static string SecretKey { get; } =
            "THIS IS USED TO SIGN AND VERIFY JWT TOKENS, REPLACE IT WITH YOUR OWN SECRET, IT CAN BE ANY STRING";

        public static DateTime TokenExpirationDate { get; } = DateTime.UtcNow.AddDays(7);

        public static string CrpytoKey { get; } = "12345678901234561234567890123456";



    }
}