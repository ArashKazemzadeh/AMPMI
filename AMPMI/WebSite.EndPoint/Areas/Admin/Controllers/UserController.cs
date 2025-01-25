using AQS_Application.Dtos.IdentityServiceDto;
using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Application.Interfaces.IServices.IdentityServices;
using AQS_Common.Enums;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Areas.Admin.Models.Company;
using WebSite.EndPoint.Utility;
using static AQS_Common.Enums.FolderNamesEnum;

namespace WebSite.EndPoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly IFileServices _fileServices;
        private readonly IRegistrationService _registrationService;
        private readonly IVideoService _videoService;
        private readonly ICompanyPictureService _companyPictureService;
        private static string PictureFolder = FolderNamesEnum.GetFileName(FolderTypes.CompanyProfile);
        static string TeaserFoldr = FolderNamesEnum.GetFileName(FolderTypes.CompanyTeaser);
        private const string Role = "Company";

        public UserController(
            ICompanyService companyService,
            IFileServices fileServices,
            IRegistrationService registeredServices,
            IVideoService videoService,
            ICompanyPictureService companyPictureService)
        {
            _companyService = companyService;
            _fileServices = fileServices;
            _registrationService = registeredServices;
            _videoService = videoService;
            _companyPictureService = companyPictureService;
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

        public async Task<IActionResult> EditUser(long id, string msg = "")
        {
            var company = await _companyService.ReadByIdAsync(id);
            if (company == null)
            {
                TempData["msg"] = "کاربر مورد نظر یافت نشد";
                return RedirectToAction(nameof(UserList));
            }
            if (!string.IsNullOrEmpty(msg))
            {
                ViewData["error"] = msg;
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
        public async Task<IActionResult> EditUser(CompanyVM companyVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(companyVM);
                }
                if (companyVM.IsLogoChanged)
                {
                    if (await DeletePicture(companyVM.LogoRout))
                        companyVM.LogoRout = null;

                    if (companyVM.Logo != null)
                    {
                        string newLogo = await AddPicture(companyVM.Logo);
                        if (string.IsNullOrEmpty(newLogo))
                        {
                            ViewData["error"] = "خطایی در هنگام دخیره لوگو رخ داد";
                            return View(companyVM);
                        }
                        companyVM.LogoRout = newLogo;
                    }
                }

                var dto = companyVM.MapToDto(companyVM);

                await _companyService.IsCompany(dto.Id, dto.IsCompany);

                var resultMessage = await _companyService.UpdateEditProfile(dto);
                if (resultMessage == ResultOutPutMethodEnum.savechanged || resultMessage == ResultOutPutMethodEnum.dontSaved)
                    return RedirectToAction(nameof(UserList));

                ViewData["error"] = resultMessage == ResultOutPutMethodEnum.savechanged ? "مشخصات ویرایش شد" :
                                    resultMessage == ResultOutPutMethodEnum.recordNotFounded ? "کاربر یافت نشد" :
                                    "مشخطات ویرایش نشد";

                return View(companyVM);
            }
            catch (Exception ex)
            {
                ViewData["error"] = ex.Message;
                return View(companyVM);
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
                if (!ModelState.IsValid)
                {
                    return View(nameof(EditUser), companyVM);
                }
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
                    return View(nameof(EditUser), companyVM);
                }
                if (companyVM.IsLogoChanged)
                {
                    if (companyVM.Logo != null)
                    {
                        string newLogo = await AddPicture(companyVM.Logo);
                        if (string.IsNullOrEmpty(newLogo))
                        {
                            ViewData["error"] = "خطایی در هنگام دخیره لوگو رخ داد";
                            return View(nameof(EditUser), companyVM);
                        }
                        companyVM.LogoRout = newLogo;
                    }
                }

                var dto = companyVM.MapToDto(companyVM);

                await _companyService.IsCompany(dto.Id, dto.IsCompany);


                return RedirectToAction(nameof(UserList));
            }
            catch (Exception ex)
            {
                ViewData["error"] = ex.Message;
                return View(nameof(EditUser), companyVM);
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

        #region CompanyTeaser
        public async Task<IActionResult> CompanyTeaser(long companyId)
        {
            var company = await _companyService.ReadByIdAsync(companyId);
            CompanyTeaserVM companyDetailVM = new CompanyTeaserVM();
            if (company != null)
            {
                companyDetailVM.CompanyName = company.Name;
                companyDetailVM.CompanyId = companyId;
                companyDetailVM.TeaserRoute = company.TeaserGuid;
            }

            return View(companyDetailVM);
        }
        [HttpPost]
        public async Task<IActionResult> CompanyTeaser(CompanyTeaserVM companyTeaserVM)
        {
            if (companyTeaserVM.Teaser == null || companyTeaserVM.CompanyId < 1)
                return RedirectToAction(nameof(CompanyTeaser));

            string msg = string.Empty;
            var company = await _companyService.ReadByIdAsync(companyTeaserVM.CompanyId);
            if (company != null)
            {
                if (!string.IsNullOrEmpty(company.TeaserGuid))
                    await _videoService.DeleteVideo(company.TeaserGuid);

                string teaserPath = await _videoService.SaveVideoAsync(companyTeaserVM.Teaser, TeaserFoldr);
                if (string.IsNullOrEmpty(teaserPath))
                {
                    msg = "خطا در هنگام ذخیره تیزر";
                }
                company.TeaserGuid = teaserPath;
                var result = await _companyService.UpdateTeaser(company);
                if (result == ResultOutPutMethodEnum.savechanged)
                    msg = "تیزر با موفقیت ذخیره شد";
                else
                    msg = "خطا در هنگام ذخیره تیزر";

            }
            return RedirectToAction(nameof(EditUser), new { id = companyTeaserVM.CompanyId, msg = msg });
        }
        public async Task<IActionResult> DeleteTeaser(long companyId)
        {

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
                        msg = "خطا در هنگام حذف تیزر ";
                }
                else
                    msg = "خطا در هنگام حذف تیزر ";
            }
            return RedirectToAction(nameof(EditUser), new { id = companyId, msg = msg });
        }

        #endregion CompanyTeaser

        #region CompanyPictures
        public async Task<IActionResult> CompanyPictures(long companyId)
        {
            var company = await _companyService.ReadByIdAsync(companyId);
            var list = await _companyPictureService.ReadAllByCompany(companyId);
            ViewData["CompanyId"] = companyId;
            ViewData["CompanyName"] = company.Name;
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
            long companyId = pictureRow.CompanyId ?? 0;
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


            return RedirectToAction(nameof(CompanyPictures), new { companyId = companyId });

        }
        [HttpPost]
        public async Task<IActionResult> NewPicture(IFormFile picture,long companyId)
        {
            if (picture == null)
                return RedirectToAction(nameof(CompanyPictures), new { companyId = companyId });

            try
            {
                string newPicture = await _fileServices.SaveFileAsync(picture, PictureFolder);
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


            return RedirectToAction(nameof(CompanyPictures), new { companyId = companyId });
        }
        #endregion CompanyPictures
    }
}
