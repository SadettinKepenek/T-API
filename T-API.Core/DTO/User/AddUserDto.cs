﻿using System;

namespace T_API.Core.DTO.User
{
    public class AddUserDto
    { 
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}