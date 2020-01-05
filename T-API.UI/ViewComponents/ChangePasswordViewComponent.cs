using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using T_API.BLL.Abstract;
using T_API.UI.Extensions;
using T_API.UI.Models.ViewComponents;

namespace T_API.UI.ViewComponents
{
    [ViewComponent(Name = "ChangePassword")]

    public class ChangePasswordViewComponent: ViewComponent
    {
        private IUserService _userService;

        public ChangePasswordViewComponent(IUserService userService)
        {
            _userService = userService;
        }


        public ViewViewComponentResult Invoke(string oldPassword)
        {
            int userId = HttpContext.GetNameIdentifier();
            ChangePasswordViewComponentModel model=new ChangePasswordViewComponentModel()
            {
                UserId = userId,
                OldPassword = oldPassword
            };
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IViewComponentResult> ChangePassword(ChangePasswordViewComponentModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _userService.ChangePassword(model.UserId, model.OldPassword, model.NewPassword);
            TempData["Message"] = "Success";
            return View();
        }
    }
}