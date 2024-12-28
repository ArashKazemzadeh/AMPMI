using AQS_Aplication.Interfaces.IServisces.BaseServices;
using AQS_Aplication.Interfaces.IServisces.IThirdParitesServices;
using AQS_Common.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using WebSite.EndPoint.Models.AcountingViewModel.Login;
using WebSite.EndPoint.Models.AcountingViewModel.Register;

namespace WebSite.EndPoint.Controllers
{
    public class LoginController : Controller //ToDo : Arash : View مونده
    {
        private readonly ISMSOTPService _smsOtpService;
        private readonly IMemoryCache _memoryCache;
        private readonly ICompanyService _companyService;
        private readonly ILoginService _loginService;
        private const int OtpExpirationSeconds = 120;
        private const string CompanyRoleName = "Company";
        private const string AdminRoleName = "Admin";
        public LoginController
            (
             ILoginService loginService,
             ISMSOTPService smsOtpService,
             IMemoryCache memoryCache,
             ICompanyService companyService
            )
        {
            _smsOtpService = smsOtpService;
            _memoryCache = memoryCache;
            _companyService = companyService;
            _companyService = companyService;
            _loginService = loginService;
        }
        public IActionResult Login()
        {
            //var model = new LoginVM() {Mobile = "" ,Password = "" };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid)
            {
                View(login);
            }

            var result = await _loginService
                .LoginWithoutRememberAsync(login.Mobile, login.Password);

            if (result.IsSuccess)
            {
                if (result.Role == AdminRoleName)
                {
                    return RedirectToAction
                     (
                      actionName: "panel",
                      controllerName: "admin",
                      routeValues: new { area = "admin", userId = result.UserId }
                     );
                }
                else
                {

                    return RedirectToAction
                       (
                        actionName: "panel",
                        controllerName: "company",
                        routeValues: new { area = "Company", userId = result.UserId }
                       );
                }
            }
            else
            {
                return View(login);
            }
        }
        /// <summary>
        /// صفحه ورود شماره موبایل
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        public IActionResult MobileInput() //کپی از رجستر
        {
            return View();
        }

        /// <summary>
        /// دریافت شماره موبایل و هدایت به صفحه تایید OTP
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> MobileInput(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
            {
                ModelState.AddModelError("mobile", "شماره موبایل نمی‌تواند خالی باشد.");
                return View();
            }
            if (!mobile.IsValidMobileNumber())
            {
                ModelState.AddModelError("mobile", "شماره موبایل نامعتبر است.");
                return View();
            }
            if (!_memoryCache.TryGetValue($"OTP_{mobile}", out int otp))
            {
                otp = await _smsOtpService.GenerateUniqueOTPAsync();
                _memoryCache.Set($"OTP_{mobile}", otp, TimeSpan.FromSeconds(OtpExpirationSeconds));
            }

            HttpStatusCode result = await _smsOtpService.SendSMSForAuthentication(mobile, otp.ToString());

            if (result == HttpStatusCode.OK)
            {
                return RedirectToAction(nameof(ConfirmOTP), new { mobile });
            }
            else
            {
                ViewData["Error"] = "ارسال کد ناموفق بود دوباره تلاش کنید";
                return View();
            }
        }

        /// <summary>
        /// نمایش صفحه تایید OTP
        /// </summary>
        /// <param name="mobile">شماره موبایل</param>
        [HttpGet]
        public IActionResult ConfirmOTP(string mobile)
        {
            if (string.IsNullOrEmpty(mobile))
            {
                return NotFound();
            }
            var model = new ConfirmOtpViewModel
            {
                Mobile = mobile,
                ErrorMessage = string.Empty
            };
            return View(model);
        }
        /// <summary>
        /// پردازش کد OTP ارسال شده
        /// </summary>
        /// <param name="model">مدل حاوی اطلاعات ورودی کاربر</param>
        [HttpPost]
        public async Task<IActionResult> ConfirmOTP(ConfirmOtpViewModel model)
        {
            if (model.UserOtp.DigitCount() != 6)
            {
                model.ErrorMessage = "تعداد رقم های کد را اشتباه وارد کرده اید";
                return View(model);
            }

            if (_memoryCache.TryGetValue($"OTP_{model.Mobile}", out int otpInMemory))
            {
                if (otpInMemory == model.UserOtp)
                {
                    var result = await _loginService.LoginWithOtp(model.Mobile);
                    if (result.Role == CompanyRoleName)
                    {
                        return RedirectToAction
                            (
                             actionName: "panel",
                             controllerName: "company",
                             routeValues: new { area = "Company", userId = result.UserId }
                            );
                    }
                    else if (result.Role == AdminRoleName)
                    {
                        return RedirectToAction
                          (
                           actionName: "panel",
                           controllerName: "admin",
                           routeValues: new { area = "admin", userId = result.UserId }
                          );
                    }
                }
                else
                {
                    model.ErrorMessage = "کد وارد شده با کد ارسال شده از طریق پیام کوتاه هم خوانی ندارد";
                    return View(model);
                }
            }
            model.ErrorMessage = "مدت زمان استفاده از کد به پایان رسیده است.";
            return View(model);
        }

        public async Task<IActionResult> LogOut()
        {
            await _loginService.LogoutAsync();
            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }
    }
}
