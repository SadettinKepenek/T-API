using System.ComponentModel.DataAnnotations;

namespace T_API.UI.Models.Security
{
    public class RegisterViewModel
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

        public string Role { get; set; }
    }
}