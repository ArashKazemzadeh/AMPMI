using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Common.Enums;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Areas.Company.Models.CompanyPicture;
using WebSite.EndPoint.Utility;

namespace WebSite.EndPoint.Areas.Company.Controllers
{
    [Area("Company")]
    public class CompanyPictureController : Controller
    {
        private readonly ICompanyPictureService _companyPictureService;
        private readonly IFileServices _fileServices;
        const string PictureFolder = "Company";
        public CompanyPictureController(ICompanyPictureService companyPictureService,
            IFileServices fileServices)
        {
            this._companyPictureService = companyPictureService;
            this._fileServices = fileServices;
        }
        public async Task<IActionResult> Pictures()
        {
            long companyId = 9;
            if (User.Identity.IsAuthenticated)
            {
                companyId = Convert.ToInt64(User.Identity.Name);
            }
            var list =await _companyPictureService.ReadAllByCompany(companyId);
            List<CompanyPictureVM> model = list.Select(x => new CompanyPictureVM() 
            {
                Id = x.Id,
                CompanyId = x.CompanyId,
                PictureFileName = x.PictureFileName
            }).ToList();

            return View(model);
        }
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _companyPictureService.Delete(id);
            if (result == ResultOutPutMethodEnum.savechanged)
                TempData["msg"] = "حذف با موفقیت انجام شد";
            else if(result == ResultOutPutMethodEnum.recordNotFounded)
                TempData["msg"] = "تصویر مورد نظر یافت نشد";    
            else
                TempData["msg"] = "خطا در هنگام حذف اطلاعات";

            return RedirectToAction(nameof(Pictures));

        }
        [HttpPost]
        public async Task<IActionResult> NewPicture(IFormFile img)
        {
            string newPicture = await _fileServices.SaveFileAsync(img, PictureFolder);
            if (string.IsNullOrEmpty(newPicture))
            {
                ViewData["msg"] = "خطایی در هنگام ثبت تصویر رخ داد";
                return View("Pictures");
            }
            else
            {
                long compnayId = 9;
                if (User.Identity.IsAuthenticated)
                {
                    compnayId = Convert.ToInt64(User.Identity.Name);
                }
                try
                {
                    if (await _companyPictureService.Create(compnayId, newPicture) > 0)
                        TempData["msg"] = "ثبت با موفقیت انجام شد";
                    else
                        TempData["msg"] = "خطا در هنگام ثبت اطلاعات";
                }
                catch (Exception)
                {
                    TempData["msg"] = "خطا در هنگام ثبت اطلاعات";
                }
            }

            return RedirectToAction(nameof(Pictures));
        }
    }
}
