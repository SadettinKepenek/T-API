﻿using System;

namespace T_API.Core.DTO.Database
{
    public class DetailDatabaseDto
    {
        public int DatabaseId { get; set; }
        public int UserId { get; set; }
        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public string Port { get; set; }
        public string Provider { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsStorageSupport { get; set; }
        public bool IsApiSupport { get; set; }
    }
}