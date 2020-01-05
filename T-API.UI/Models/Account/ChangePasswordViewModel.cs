using System.ComponentModel.DataAnnotations;

namespace T_API.UI.Models.Account
{
    public class ChangePasswordViewModel
    {
        [Required] public string OldPassword { get; set; }
        [Required]
        [Compare(nameof(NewPassword2), ErrorMessage = "Two Passwords are not matched.")]
        public string NewPassword { get; set; }
        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "Two Passwords are not matched.")]
        public string NewPassword2 { get; set; }

    }
}