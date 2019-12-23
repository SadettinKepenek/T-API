using System.ComponentModel.DataAnnotations;
using T_API.Core.DTO.User;

namespace T_API.UI.Models.Account
{
    public class SettingsViewModel
    {
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
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Role { get; set; }
    }
}