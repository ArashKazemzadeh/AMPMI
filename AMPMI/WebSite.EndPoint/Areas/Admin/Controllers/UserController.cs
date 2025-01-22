using AQS_Application.Dtos.IdentityServiceDto;
using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Application.Interfaces.IServices.IdentityServices;
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
        private readonly IRegistrationService _registrationService;
        private const string PictureFolder = "CompanyProfile";
        private const string Role = "Company";

        public UserController(ICompanyService companyService, IFileServices fileServices, IRegistrationService registeredServices)
        {
            _companyService = companyService;
            _fileServices = fileServices;
            _registrationService = registeredServices;
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
            var resultReg = await _registrationService.DeleteUserAsync(id);
            if (!resultReg)
            {
                TempData["msg"] = "خطا در هنگام حذف کاربر";
                return RedirectToAction(nameof(UserList));
            }
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
                Tel = company.Tel
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(CompanyVM CompanyVM)
        {
            try
            {
                if (CompanyVM.IsLogoChanged)
                {
                    if (await DeletePicture(CompanyVM.LogoRout))
                        CompanyVM.LogoRout = null;

                    if (CompanyVM.Logo != null)
                    {
                        string newLogo = await AddPicture(CompanyVM.Logo);
                        if (string.IsNullOrEmpty(newLogo))
                        {
                            ViewData["error"] = "خطایی در هنگام دخیره لوگو رخ داد";
                            return RedirectToAction(nameof(EditUser), new { id = CompanyVM.Id });
                        }
                        CompanyVM.LogoRout = newLogo;
                    }
                }

                var dto = CompanyVM.MapToDto(CompanyVM);

                await _companyService.IsCompany(dto.Id, dto.IsCompany);

                var resultMessage = await _companyService.UpdateEditProfile(dto);
                if (resultMessage == ResultOutPutMethodEnum.savechanged || resultMessage == ResultOutPutMethodEnum.dontSaved)
                    return RedirectToAction(nameof(UserList));

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
        public async Task<IActionResult> CreateUser()
        {
            return View("EditUser", new CompanyVM());
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CompanyVM companyVM)
        {
            try
            {
                RegisterIdentityDTO registerIdentityDTO = new()
                {

                    Mobile = companyVM.MobileNumber,
                    CompanyName = companyVM.Name,
                    ManagerName = companyVM.ManagerName,
                    Email = companyVM.Email,
                    Password = companyVM.MobileNumber,
                    Address = companyVM.Address
                };

                var createNew = await _registrationService.RegisterAsync(registerIdentityDTO, Role, false);
                if (createNew.userId <= 0)
                {
                    ViewData["error"] = createNew.errorMessage;
                    return RedirectToAction(nameof(EditUser), new { id = createNew.userId });
                }
                if (companyVM.IsLogoChanged)
                {
                    if (companyVM.Logo != null)
                    {
                        string newLogo = await AddPicture(companyVM.Logo);
                        if (string.IsNullOrEmpty(newLogo))
                        {
                            ViewData["error"] = "خطایی در هنگام دخیره لوگو رخ داد";
                            return RedirectToAction(nameof(EditUser), new { id = createNew.userId });
                        }
                        companyVM.LogoRout = newLogo;
                    }
                }

                var dto = companyVM.MapToDto(companyVM);

                await _companyService.IsCompany(dto.Id, dto.IsCompany);


                return RedirectToAction(nameof(EditUser), new { id = createNew.userId });
            }
            catch (Exception ex)
            {
                ViewData["error"] = ex.Message;
                return RedirectToAction(nameof(EditUser), new { id = 0 });
            }
        }
        private async Task<bool> DeletePicture(string route)
        {
            return await _fileServices.DeleteFile(route);
        }
        private async Task<string> AddPicture(IFormFile newPicture)
        {
            return await _fileServices.SaveFileAsync(newPicture, PictureFolder);
        }
        [HttpPost]
        public async Task<IActionResult> Save(CompanyVM companyVM)
        {
            if (companyVM.Id > 0)
                return await EditUser(companyVM);
            else
                return await CreateUser(companyVM);
        }
    }
}
