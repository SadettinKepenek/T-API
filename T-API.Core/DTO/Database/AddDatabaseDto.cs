using System;
using System.Collections.Generic;
using T_API.Core.DAL.Concrete;
using T_API.Core.DTO.Table;

namespace T_API.Core.DTO.Database
{
    public class AddDatabaseDto
    {
        public int UserId { get; set; }
        public string Server { get; set; }
        public string DatabaseName { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
        public string Provider { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int PackageId { get; set; }
        public int MonthCount { get; set; }

    }
}