using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using T_API.UI.Models.ViewComponents;

namespace T_API.UI.ViewComponents
{
    [ViewComponent(Name = "ChangePassword")]

    public class ChangePasswordViewComponent: ViewComponent
    {
        public ViewViewComponentResult Invoke()
        {

            return View();
        }

    }
}