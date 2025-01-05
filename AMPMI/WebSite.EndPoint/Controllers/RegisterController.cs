using AQS_Application.Dtos.IdentityServiceDto;
using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Application.Interfaces.IServices.IdentityServices;
using AQS_Application.Interfaces.IServices.IThirdParitesServices;
using AQS_Common.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using WebSite.EndPoint.Models.AccountingViewModel.Register;

namespace WebSite.EndPoint.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ISMSOTPService _smsOtpService;
        private readonly IMemoryCache _memoryCache;
        private readonly IRegistrationService _registrationService;
        private readonly ICompanyService _companyService;
        private const int OtpExpirationSeconds = 120;
        private const string CompanyRole = "Company";

        public RegisterController
            (
            ISMSOTPService smsOtpService,
            IMemoryCache memoryCache,
            IRegistrationService registrationService,
            ICompanyService companyService
            )
        {
            _smsOtpService = smsOtpService;
            _memoryCache = memoryCache;
            _registrationService = registrationService;
            _companyService = companyService;
        }

        /// <summary>
        /// صفحه ورود شماره موبایل
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        public IActionResult MobileInput()
        {
            return View();
        }

        /// <summary>
        /// دریافت شماره موبایل و هدایت به صفحه تایید OTP
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> MobileInput(string mobile) //Ok
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
            if (await _companyService.IsExistByMobileNumber(mobile))
            {
                ModelState.AddModelError("mobile", "شماره موبایل قبلا ثبت شده است.");
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
        public IActionResult ConfirmOTP(string mobile) //ok
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
        public IActionResult ConfirmOTP(ConfirmOtpViewModel model) //ok
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
                    return RedirectToAction(nameof(Register),new { model.Mobile } );
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
        /// <summary>
        /// صفحه ثبت نام
        /// </summary>
        public IActionResult Register(string mobile) //ok
        {
            if (string.IsNullOrEmpty(mobile))
            {
                return NotFound();
            }
            var newModel = CreateCompanyMV.NewObject(mobile);
            return View(newModel);
        }
        [HttpPost]
        public async Task<IActionResult> Register(CreateCompanyMV createCompany) //ok
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(createCompany);

                RegisterIdentityDTO newCompany = createCompany.ConvertToRegisterIdentityDTO();

                if (newCompany == null)
                    return View(createCompany);

                var result = await _registrationService.RegisterAsync(newCompany, CompanyRole);

                if (result.userId > 0 && string.IsNullOrEmpty(result.errorMessage))
                {
                    return RedirectToAction("SuccessPage", new { id = result.userId });
                }
                else
                {
                    // تقسیم خطاها و افزودن به ModelState
                    string[] errors = result.errorMessage.Split(',');
                    foreach (var error in errors)
                    {
                        if (error.Contains("ایمیل"))
                            ModelState.AddModelError("Email", error.Trim());
                        else if (error.Contains("شماره موبایل"))
                            ModelState.AddModelError("Mobile", error.Trim());
                        else if (error.Contains("رمز عبور"))
                            ModelState.AddModelError("Password", error.Trim());
                        else
                            ModelState.AddModelError(string.Empty, error.Trim());
                    }

                    return Redirect("/home/index/");
                }
            }
            catch (Exception)
            {
                //ToDo : ویو مدل
                return View(createCompany);
            }
           
        }
    }
}
