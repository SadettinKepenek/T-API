using System.ComponentModel.DataAnnotations;

namespace T_API.UI.Areas.Admin.Models.User
{
    public class CreateUserViewModel
    {
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}