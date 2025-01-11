using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Common.Enums;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Areas.Admin.Models.Company;
using WebSite.EndPoint.Utility;

namespace WebSite.EndPoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly IFileServices _fileServices;
        private const string PictureFolder = "CompanyProfile";

        public UserController(ICompanyService companyService, IFileServices fileServices)
        {
            _companyService = companyService;
            _fileServices = fileServices;
        }
        public async Task<IActionResult> UserList()
        {
            var company = await _companyService.Read();

            var model = company.Select(c => new CompanyVM
            {
                Id = c.Id,
                Name = c.Name,
                ManagerName = c.ManagerName,
                MobileNumber = c.MobileNumber,
                Email = c.Email,
                IsCompany = c.IsCompany,
                SendRequst = c.SendRequst
            }).ToList();
            return View(model);
        }
        public async Task<IActionResult> DeleteUser(long id)
        {
            var result = await _companyService.Delete(id);
            if (result == ResultOutPutMethodEnum.savechanged)
                TempData["msg"] = "حذف با موفقیت انجام شد";
            else if (result == ResultOutPutMethodEnum.recordNotFounded)
                TempData["msg"] = "کاربر مورد نظر یافت نشد";
            else
                TempData["msg"] = "خطا در هنگام حذف اطلاعات";

            return RedirectToAction(nameof(UserList));
        }

        public async Task<IActionResult> EditUser(long id)
        {
            var company = await _companyService.ReadByIdAsync(id);
            if (company == null)
            {
                TempData["msg"] = "کاربر مورد نظر یافت نشد";
                return RedirectToAction(nameof(UserList));
            }
            var model = new CompanyVM
            {
                Id = company.Id,
                Name = company.Name,
                ManagerName = company.ManagerName,
                MobileNumber = company.MobileNumber,
                Email = company.Email,
                Address = company.Address,
                Brands = company.Brands,
                Capacity = company.Capacity,
                Partnership = company.Partnership,
                QualityGrade = company.QualityGrade,
                Iso = company.Iso,
                About = company.About,
                LogoRout = company.LogoRout == null ? string.Empty : company.LogoRout,
                IsCompany = company.IsCompany,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(CompanyVM CompanyVM)
        {
            try
            {
                string newRout = "";
                if (CompanyVM.IsPictureChanged)
                {
                    if (!string.IsNullOrEmpty(CompanyVM.LogoRout))
                    {
                        bool isDelete = await _fileServices.DeleteFile(CompanyVM.LogoRout);
                    }

                    newRout = await _fileServices.SaveFileAsync(CompanyVM.Logo, PictureFolder);

                    if (string.IsNullOrEmpty(newRout))
                    {
                        ViewData["error"] = "خطایی در هنگام دخیره تصویر رخ داد";
                        return RedirectToAction(nameof(UserList));
                    }

                    CompanyVM.LogoRout = newRout;
                }

                var dto = CompanyVM.MapToDto(CompanyVM);

                if(dto.IsCompany)
                await _companyService.IsCompany(dto.Id , dto.IsCompany);

                var resultMessage = await _companyService.UpdateEditProfile(dto);
                ViewData["error"] = resultMessage == ResultOutPutMethodEnum.savechanged ? "مشخصات ویرایش شد" :
                                    resultMessage == ResultOutPutMethodEnum.recordNotFounded ? "کاربر یافت نشد" :
                                    "مشخطات ویرایش نشد";

                return View(CompanyVM);
            }
            catch (Exception ex)
            {
                ViewData["error"] = ex.Message;
                return View(CompanyVM);
            }
        }
    }
}
