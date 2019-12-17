using System;
using System.Collections.Generic;
using T_API.Core.DTO.Database;

namespace T_API.Core.DTO.User
{
    public class DetailUserDto
    {
        public Guid UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public List<ListDatabaseDto> Databases { get; set; }
        public bool IsActive { get; set; }
    }
}