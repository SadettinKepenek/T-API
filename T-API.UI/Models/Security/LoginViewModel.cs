using System.ComponentModel.DataAnnotations;

namespace T_API.UI.Models.Security
{
    public class LoginViewModel
    {
        [Required]

        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}