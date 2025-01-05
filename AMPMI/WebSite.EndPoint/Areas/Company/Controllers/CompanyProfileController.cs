using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Common.Enums;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Utility;

namespace WebSite.EndPoint.Areas.Company.Controllers
{
    [Area("Company")]
    public class CompanyProfileController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly IVideoService _videosService;

        const string TeaserPath = "Teaser";
        public CompanyProfileController(ICompanyService companyService,IVideoService videoService)
        {
            this._companyService = companyService;        
            this._videosService = videoService;
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
                try
                {
                    string teaserPath = await _videosService.SaveVideoAsync(teaser, TeaserPath);
                    if (!string.IsNullOrEmpty(teaserPath))
                    {
                        company.TeaserGuid = teaserPath;
                        var result = await _companyService.Update(company);
                        if (result == ResultOutPutMethodEnum.savechanged)
                            msg = "تغییرات با موفقیت ذخیره شد";
                        else
                            msg = "خطا در هنگام ذخیره اطلاعات ";
                    }
                }
                catch (Exception)
                {
                    msg = "خطا در هنگام ذخیره اطلاعات ";
                }
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
                try
                {
                    if (_videosService.DeleteVideo(company.TeaserGuid))
                    {
                        company.TeaserGuid = string.Empty;
                        var result = await _companyService.Update(company);
                        if (result == ResultOutPutMethodEnum.savechanged)
                            msg = "تغییرات با موفقیت ذخیره شد";
                        else
                            msg = "خطا در هنگام ذخیره اطلاعات ";
                    }
                    else
                        msg = "ویدیو مورد نظر یافت نشد";
                }
                catch (Exception)
                {
                    msg = "خطا در هنگام ذخیره اطلاعات ";
                }
            }
            return RedirectToAction(nameof(EditTeaser), new { msg = msg });
        }
    }
}
