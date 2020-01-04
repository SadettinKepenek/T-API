using System;
using System.Collections.Generic;
using T_API.Core.DTO.DatabasePackage;
using T_API.Core.DTO.Table;

namespace T_API.Core.DTO.Database
{
    public class DetailDatabaseDto
    {
        public int DatabaseId { get; set; }
        public int UserId { get; set; }
        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
        public string Port { get; set; }
        public string Provider { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsStorageSupport { get; set; }
        public bool IsApiSupport { get; set; }

        public List<DetailTableDto> Tables { get; set; }
        public DetailDatabasePackageDto Package { get; set; }
    }
}