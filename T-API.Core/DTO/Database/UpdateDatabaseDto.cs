﻿using System;
using T_API.Core.DAL.Concrete;

namespace T_API.Core.DTO.Database
{
    public class UpdateDatabaseDto
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
        public int PackageId { get; set; }

    }
}