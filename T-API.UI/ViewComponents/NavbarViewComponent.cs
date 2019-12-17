using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using T_API.UI.Models.ViewComponents;

namespace T_API.UI.ViewComponents
{
    [ViewComponent(Name = "Navbar")]
    public class NavbarViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NavbarViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ViewViewComponentResult Invoke()
        {
            var user = _httpContextAccessor.HttpContext.User;
            NavbarViewComponentModel model = new NavbarViewComponentModel()
            {
                User = user
            };
            return View(model);
        }

    }
}