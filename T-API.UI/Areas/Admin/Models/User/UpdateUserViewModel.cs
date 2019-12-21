using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace T_API.UI.Areas.Admin.Models.User
{
    public class UpdateUserViewModel
    {
        public int UserId { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
