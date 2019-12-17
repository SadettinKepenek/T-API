using System.Security.Claims;

namespace T_API.UI.Models.ViewComponents
{
    public class NavbarViewComponentModel
    {
        public ClaimsPrincipal User { get; set; }
    }
}