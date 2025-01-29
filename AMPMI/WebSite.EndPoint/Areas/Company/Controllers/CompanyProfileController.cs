using AQS_Application.Dtos.BaseServiceDto.Company;
using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Application.Interfaces.IServices.IdentityServices;
using AQS_Common.Enums;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Areas.Company.Models.Company;
using WebSite.EndPoint.Areas.Company.Models.Profile;
using WebSite.EndPoint.Utility;
using static AQS_Common.Enums.FolderNamesEnum;

namespace WebSite.EndPoint.Areas.Company.Controllers
{
    [Area("Company")]
    [Authorize(Roles = "Admin,Company")]
    public class CompanyProfileController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly IRegistrationService _registrationService;
        private readonly ILoginService _loginService;
        private readonly IFileServices _fileServices;
        private readonly IVideoService _videoService;

        static string PictureFolder = FolderNamesEnum.GetFileName(FolderTypes.CompanyProfile);
        static string TeaserFoldr = FolderNamesEnum.GetFileName(FolderTypes.CompanyTeaser);
        public CompanyProfileController(
            ICompanyService companyService,
            IRegistrationService registrationService,
            ILoginService loginService,
            IFileServices fileServices,
            IVideoService videoService)
        {
            _companyService = companyService;
            _registrationService = registrationService;
            _loginService = loginService;
            _fileServices = fileServices;
            _videoService = videoService;
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
        public async Task<IActionResult> EditCompanyProfile()
        {
            if (TempData["SendRequestToAdmin"] is not null)
                ViewData["error"] = TempData["SendRequestToAdmin"];

            CompanyEditProfileVM model = await EditProfileModelCreator();

            return View(model);
        }

        private async Task<CompanyEditProfileVM> EditProfileModelCreator()
        {
            long companyId = await _loginService.GetUserIdAsync(User);

            var company = await _companyService.ReadByIdAsync(companyId);
            if (company == null)
            {
                ViewData["error"] = "کاربر یافت نشد";
                return new CompanyEditProfileVM();
            }
            var model = new CompanyEditProfileVM

            {
                Id = companyId,
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
                SendRequest = company.SendRequest,
                IsCompany = company.IsCompany,
                Tel = company.Tel,
                Website = company.Website
            };
            return model;
        }

        [HttpPost]
        public async Task<IActionResult> EditCompanyProfile(CompanyEditProfileVM companyEditProfileVM) //ToDo
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(nameof(EditCompanyProfile), companyEditProfileVM);
                }
                if (companyEditProfileVM.IsLogoChanged)
                {
                    if( await DeletePicture(companyEditProfileVM.LogoRout))
                        companyEditProfileVM.LogoRout = null;

                    if (companyEditProfileVM.Logo != null)
                    {
                       string newLogo  = await AddPicture(companyEditProfileVM.Logo);
                       if (string.IsNullOrEmpty(newLogo))
                       {
                           ViewData["error"] = "خطایی در هنگام دخیره لوگو رخ داد";
                           return View(nameof(EditCompanyProfile),companyEditProfileVM);
                       }
                       companyEditProfileVM.LogoRout = newLogo;
                    }
                }

                var dto = companyEditProfileVM.MapToDto(companyEditProfileVM);

                var resultMessage = await _companyService.UpdateEditProfile(dto);
                ViewData["error"] = resultMessage == ResultOutPutMethodEnum.savechanged ? "مشخصات ویرایش شد" :
                                    resultMessage == ResultOutPutMethodEnum.recordNotFounded ? "کاربر یافت نشد" :
                                    "مشخطات ویرایش نشد";

                if (ResultOutPutMethodEnum.savechanged == resultMessage)
                {
                    return Redirect("/Company/CompanyPanel/Panel");
                }

                return View(companyEditProfileVM);
            }
            catch (Exception ex)
            {
                ViewData["error"] = ex.Message;
                return View(companyEditProfileVM);
            }
        }
        private async Task<bool> DeletePicture(string route)
        {
            return await _fileServices.DeleteFile(route);
        }
        private async Task<string> AddPicture(IFormFile newPicture)
        {
            return  await _fileServices.SaveFileAsync(newPicture, PictureFolder);
        }
        public async Task<IActionResult> SendRequestToAdmin(long id, bool sendRequest)
        {
            var resultMessage = await _companyService.SendRequest(id, sendRequest);

            TempData["SendRequestToAdmin"] =
                                resultMessage == ResultOutPutMethodEnum.savechanged ? "درخواست ارسال شد" :
                                resultMessage == ResultOutPutMethodEnum.recordNotFounded ? "مشخصات کاربری یافت نشد." :
                                resultMessage == ResultOutPutMethodEnum.duplicateRecord ? "شما قبلا درخواست داده اید"
                                : "ارسال درخواست با مشکل مواجه شد";

            return RedirectToAction(nameof(EditCompanyProfile));
        }

        
        public async Task<IActionResult> EditTeaser(string msg = "")
        {
            if (!string.IsNullOrEmpty(msg))
            {
                ViewData["msg"] = msg;
            }

            long companyId = await _loginService.GetUserIdAsync(User);
            var company = await _companyService.ReadByIdAsync(companyId);
            if (company != null)
                ViewData["src"] = company.TeaserGuid;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditTeaser(IFormFile teaser)
        {
            if (teaser == null)
                return RedirectToAction(nameof(EditTeaser));

            long companyId = await _loginService.GetUserIdAsync(User);

            string msg = string.Empty;
            var company = await _companyService.ReadByIdAsync(companyId);
            if (company != null)
            {
                string teaserPath = await _videoService.SaveVideoAsync(teaser, TeaserFoldr);
                if (string.IsNullOrEmpty(teaserPath))
                {
                    msg = "خطا در هنگام ذخیره ویدیو";
                }
                company.TeaserGuid = teaserPath;
                var result = await _companyService.UpdateTeaser(company);
                if (result == ResultOutPutMethodEnum.savechanged)
                    msg = "تغییرات با موفقیت ذخیره شد";
                else
                    msg = "خطا در هنگام ذخیره اطلاعات ";
            }
            return RedirectToAction(nameof(EditTeaser), new { msg = msg });
        }
        public async Task<IActionResult> DeleteTeaser()//OK
        {

            long companyId = await _loginService.GetUserIdAsync(User);
            string msg = string.Empty;
            var company = await _companyService.ReadByIdAsync(companyId);
            if (company != null && !string.IsNullOrEmpty(company.TeaserGuid))
            {
                if (await _videoService.DeleteVideo(company.TeaserGuid))
                {
                    company.TeaserGuid = string.Empty;
                    var result = await _companyService.UpdateTeaser(company);
                    if (result == ResultOutPutMethodEnum.savechanged)
                        msg = "تغییرات با موفقیت ذخیره شد";
                    else
                        msg = "خطا در هنگام ذخیره اطلاعات ";
                }
                else
                    msg = "خطا در هنگام ذخیره اطلاعات ";
            }
            return RedirectToAction(nameof(EditTeaser), new { msg = msg });
        }
    }
}
