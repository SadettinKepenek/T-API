using System.ComponentModel.DataAnnotations;

namespace T_API.UI.Models.ViewComponents
{
    public class ChangePasswordViewComponentModel
    {
        [Required] public int UserId { get; set; }
        [Required] public string OldPassword { get; set; }
        [Required]
        [Compare(nameof(NewPassword2), ErrorMessage = "Two Passwords are not matched.")]
        public string NewPassword { get; set; }
        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "Two Passwords are not matched.")]
        public string NewPassword2 { get; set; }
    }
}