using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Areas.Company.Controllers
{
    [Area("Company")]
    public class CompanyProfileController : Controller
    {
        private readonly ICompanyService _companyService;
        public CompanyProfileController(ICompanyService companyService)
        {
            this._companyService = companyService;        
        }
        public IActionResult EditCompanyProfile()
        {
            return View();
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        public async Task<IActionResult> EditTeaser(string msg="")
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
        public async Task<IActionResult> EditTeaser(IFormFile teaser)
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
        public async Task<IActionResult> DeleteTeaser()
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
