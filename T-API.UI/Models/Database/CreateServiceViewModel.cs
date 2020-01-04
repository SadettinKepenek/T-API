﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using T_API.Core.DTO.Table;

namespace T_API.UI.Models.Database
{
    public class CreateServiceViewModel
    {
        public int UserId { get; set; }
        public string Server { get; set; }
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
        [Required] public string DatabaseName { get; set; }
        public string Port { get; set; }
        public string Provider { get; set; }

        public bool IsActive { get; set; }
        public int PackageId { get; set; }
    }
}