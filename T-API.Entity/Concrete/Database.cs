using System;
using System.Collections.Generic;

namespace T_API.Entity.Concrete
{
    public class Database
    {
        private int databaseId;

        public int DatabaseId
        {
            get { return databaseId; }
            set { databaseId = value; }
        }

        private int userId;

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
        public string Port { get; set; }
        public string Provider { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int PackageId { get; set; }
        public List<Table> Tables { get; set; }
        public DatabasePackage Package { get; set; }
    }
}