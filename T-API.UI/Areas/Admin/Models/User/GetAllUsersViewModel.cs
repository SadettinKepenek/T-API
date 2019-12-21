using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T_API.UI.Areas.Admin.Models.User
{
    public class GetAllUsersViewModel
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
