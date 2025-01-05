using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Application.Interfaces.IServices.IdentityServices;
using AQS_Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Areas.Company.Models;
using WebSite.EndPoint.Areas.Company.Models.Profile;

namespace WebSite.EndPoint.Areas.Company.Controllers
{
    [Area("Company")]
    //[Authorize]
    public class CompanyProfileController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly IRegistrationService _registrationService;
        private readonly ILoginService _loginService;

        public CompanyProfileController(
            ICompanyService companyService,
            IRegistrationService registrationService,
            ILoginService loginService)
        {
            _companyService = companyService;
            _registrationService = registrationService;
            _loginService = loginService;
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

            if (result.userId == 0 )
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
        public IActionResult EditCompanyProfile()
        {
            return View();
        }
        public async Task<IActionResult> EditTeaser(string msg = "")//OK
        {
            if (!string.IsNullOrEmpty(msg))
            {
                ViewData["msg"] = msg;
            }
            long companyId = 9;//Todo
            if (User.Identity.IsAuthenticated)
            {
                companyId = await _loginService.GetUserIdAsync(User);
            }
            var company = await _companyService.ReadById(companyId);
            if (company != null)
            {
                ViewData["src"] = company.TeaserGuid;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditTeaser(IFormFile teaser) //OK
        {
            if (teaser == null)
            {
                return View();
            }
            long companyId = 9;
            if (User.Identity.IsAuthenticated)
            {
                companyId = await _loginService.GetUserIdAsync(User);
            }
            string msg = string.Empty;
            var company = await _companyService.ReadById(companyId);
            if (company != null)
            {
                company.TeaserGuid = "Something"; // TODO : Video Service is unavailable
                var result = await _companyService.Update(company);
                if (result == ResultOutPutMethodEnum.savechanged)
                    msg = "تغییرات با موفقیت ذخیره شد";
                else
                    msg = "خطا در هنگام ذخیره اطلاعات ";
            }
            return RedirectToAction(nameof(EditTeaser), new { msg = msg });
        }
        public async Task<IActionResult> DeleteTeaser()//OK
        {
            long companyId = 9;
            if (User.Identity.IsAuthenticated)
            {
                companyId = await _loginService.GetUserIdAsync(User);
            }
            string msg = string.Empty;
            var company = await _companyService.ReadById(companyId);
            if (company != null)
            {
                company.TeaserGuid = string.Empty;
                var result = await _companyService.Update(company);
                if (result == ResultOutPutMethodEnum.savechanged)
                    msg = "تغییرات با موفقیت ذخیره شد";
                else
                    msg = "خطا در هنگام ذخیره اطلاعات ";
            }
            return RedirectToAction(nameof(EditTeaser), new { msg = msg });
        }
    }
}
