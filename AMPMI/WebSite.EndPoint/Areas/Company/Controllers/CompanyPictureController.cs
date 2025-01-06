using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Application.Interfaces.IServices.IdentityServices;
using AQS_Application.Services;
using AQS_Common.Enums;
using Domin.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Areas.Company.Models.CompanyPicture;
using WebSite.EndPoint.Utility;

namespace WebSite.EndPoint.Areas.Company.Controllers
{
    [Area("Company")]
    [Authorize]
    public class CompanyPictureController : Controller
    {
        private readonly ICompanyPictureService _companyPictureService;
        private readonly IFileServices _fileServices;
        private readonly ILoginService _loginService;
        const string PictureFolder = "Company";
        public CompanyPictureController(ICompanyPictureService companyPictureService,
            IFileServices fileServices, ILoginService loginService)
        {
            this._companyPictureService = companyPictureService;
            this._fileServices = fileServices;
            this._loginService = loginService;
        }
        public async Task<IActionResult> Pictures()
        {
            long companyId = await _loginService.GetUserIdAsync(User);
            var list = await _companyPictureService.ReadAllByCompany(companyId);
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
            var pictureRow = await _companyPictureService.ReadById(id);
            if (pictureRow != null && await _fileServices.DeleteFile(pictureRow.PictureFileName))
            {
                var result = await _companyPictureService.Delete(id);
                if (result == ResultOutPutMethodEnum.savechanged)
                    TempData["msg"] = "حذف با موفقیت انجام شد";
                else if (result == ResultOutPutMethodEnum.recordNotFounded)
                    TempData["msg"] = "تصویر مورد نظر یافت نشد";
                else
                    TempData["msg"] = "خطا در هنگام حذف اطلاعات";
            }
            else
                TempData["msg"] = "دیتای مورد نظر یافت نشد";


            return RedirectToAction(nameof(Pictures));

        }
        [HttpPost]
        public async Task<IActionResult> NewPicture(IFormFile img)
        {
            long companyId = await _loginService.GetUserIdAsync(User);
            try
            {
                string newPicture = await _fileServices.SaveFileAsync(img, PictureFolder);
                if (string.IsNullOrEmpty(newPicture))
                {
                    ViewData["msg"] = "خطایی در هنگام ثبت تصویر رخ داد";
                    return View("Pictures");
                }
                if (await _companyPictureService.Create(companyId, newPicture) > 0)
                    TempData["msg"] = "ثبت با موفقیت انجام شد";
                else
                    TempData["msg"] = "خطا در هنگام ثبت اطلاعات";
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }


            return RedirectToAction(nameof(Pictures));
        }
    }
}