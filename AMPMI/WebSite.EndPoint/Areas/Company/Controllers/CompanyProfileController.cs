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
    public class CompanyProfileController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly IRegistrationService _registrationService;

        public CompanyProfileController(ICompanyService companyService, IRegistrationService registrationService)
        {
            this._companyService = companyService;
            _registrationService = registrationService;
        }
        public IActionResult EditCompanyProfile()
        {
            return View();
        }
        public async Task<IActionResult> ChangePassword()
        {
            var model = new PasswordEditVm();
            return View(model);
        }

        
        public async Task<IActionResult> ChangePassword(PasswordEditVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var companyId = Convert.ToInt64(User.Identity.Name);
            var result = await _registrationService
                .ChangePasswordAsync(companyId, model.CurrentPassword, model.NewPassword);
            if (result.userId < 1)
            {
                ViewData["Message"] = result.errorMessage;
                return View(model);
            }
            return Redirect("/company/CompanyPanel/panel");
        }
        public async Task<IActionResult> EditTeaser(string msg = "")//OK
        {
            if (!string.IsNullOrEmpty(msg))
            {
                ViewData["msg"] = msg;
            }
            long companyId = 9;
            if (User.Identity.IsAuthenticated)
            {
                companyId = Convert.ToInt64(User.Identity.Name);
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
                companyId = Convert.ToInt64(User.Identity.Name);
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
                companyId = Convert.ToInt64(User.Identity.Name);
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
