using System;
using System.Collections.Generic;

namespace T_API.Core.DTO.User
{
    public class ListUserDto
    {
        public int UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; }

    }
}