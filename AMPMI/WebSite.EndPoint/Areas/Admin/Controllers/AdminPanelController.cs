using AQS_Application.Interfaces.IServices.IdentityServices;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Areas.Company.Models.Profile;

namespace WebSite.EndPoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminPanelController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IRegistrationService _registrationService;
        public AdminPanelController(ILoginService loginService,
            IRegistrationService registrationService)
        {
            _loginService = loginService;
            _registrationService = registrationService;
        }
        public IActionResult Panel()
        {
            return View();
        }
        public Task<IActionResult> ChangePassword()
        {
            var model = new PasswordEditVm();
            return Task.FromResult<IActionResult>(View(model));
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordEditVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.CurrentPassword == model.NewPassword)
            {
                ViewData["error"] = "گذرواژه فعلی و رمز عبور جدید نمی‌توانند یکسان باشند";
                return View(model);
            }

            long companyId = 0;
            if (User.Identity.IsAuthenticated)
            {
                companyId = await _loginService.GetUserIdAsync(User);
            }
            else /*if (companyId < 1)*/
            {
                ViewData["error"] = "شما نیاز به ورود مجدد دارید. مدت زمان شما به پایان رسیده است.";
                return View(model);
            }
            var checkPassword = await _loginService.IsValidPassword(companyId, model.CurrentPassword);
            if (!checkPassword)
            {
                ViewData["error"] = "گذرواژه فعلی صحیح نیست";
                return View(model);
            }
            var result = await _registrationService
                .ChangePasswordAsync(companyId, model.CurrentPassword, model.NewPassword);

            if (result.userId == 0)
            {
                ViewData["error"] = result.errorMessage;
                return View(model);
            }
            else
            {
                await _loginService.LogoutAsync();
                return Redirect("/login/login");
            }
        }
    }
}
