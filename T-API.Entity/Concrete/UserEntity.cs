using System;
using System.Collections.Generic;

namespace T_API.Entity.Concrete
{
    public class UserEntity
    {
        public Guid UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public List<DatabaseEntity> Databases { get; set; }
        public bool IsActive { get; set; }

    }
}