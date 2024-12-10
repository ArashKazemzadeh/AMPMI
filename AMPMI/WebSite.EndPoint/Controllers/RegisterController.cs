using AQS_Aplication.Interfaces.IServisces.IdentityServices;
using AQS_Aplication.Interfaces.IServisces.IThirdParitesServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebSite.EndPoint.Models.AcountingViewModel.Register;

namespace WebSite.EndPoint.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ISMSOTPService _smsOtpService;
        private readonly IMemoryCache _memoryCache;
        private readonly IRegistrationService _registrationService;
        private const int OtpExpirationSeconds = 30; // زمان اعتبار OTP

        public RegisterController(ISMSOTPService smsOtpService, IMemoryCache memoryCache, IRegistrationService registrationService)
        {
            _smsOtpService = smsOtpService;
            _memoryCache = memoryCache;
            _registrationService = registrationService;
        }

        /// <summary>
        /// صفحه ورود شماره موبایل
        /// </summary>
        /// <returns>View</returns>
        public IActionResult MobileInput()
        {
            return View();
        }
        /// <summary>
        /// دریافت شماره موبایل و هدایت به صفحه تایید OTP
        /// </summary>
        [HttpPost]
        public IActionResult MobileInput(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
                return View();

            return RedirectToAction(nameof(ConfirmOTP), new { mobile });
        }

        /// <summary>
        /// نمایش صفحه تایید OTP
        /// </summary>
        /// <param name="mobile">شماره موبایل</param>
        /// <param name="errorMsg">پیام خطا</param>
        public async Task<IActionResult> ConfirmOTP(string mobile, string errorMsg = "")
        {
            if (string.IsNullOrWhiteSpace(mobile))
                return RedirectToAction(nameof(MobileInput));

            ViewData["Mobile"] = mobile;

            if (!string.IsNullOrWhiteSpace(errorMsg))
                ViewData["Error"] = errorMsg;

            // بررسی و ذخیره OTP در Cache
            if (!_memoryCache.TryGetValue($"OTP_{mobile}", out int otp))
            {
                otp = await _smsOtpService.GenerateUniqueOTPAsync();

                // ذخیره OTP در Cache
                _memoryCache.Set($"OTP_{mobile}", otp, TimeSpan.FromSeconds(OtpExpirationSeconds));
            }

            //await _smsOtpService.SendSMSForAuthentication(mobile, otp.ToString());

            return View();
        }
        /// <summary>
        /// بررسی OTP
        /// </summary>
        [HttpPost]
        public IActionResult ValidateOTP(string mobile, int userOtp)
        {
            if (string.IsNullOrWhiteSpace(mobile))
                return RedirectToAction(nameof(MobileInput));

            // بررسی OTP ذخیره‌شده در Cache
            if (_memoryCache.TryGetValue($"OTP_{mobile}", out int storedOtp) && storedOtp == userOtp)
                return RedirectToAction(nameof(Register));

            return RedirectToAction(nameof(ConfirmOTP), new { mobile, errorMsg = "کد تایید اشتباه است" });
        }

        /// <summary>
        /// صفحه ثبت نام
        /// </summary>
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(CreateCompanyMV createCompany)
        {
            if (!ModelState.IsValid)
            {
                return View(createCompany);
            }

            var result = await _registrationService.RegisterAsync(createCompany.ToRegisterIdentityDTO(), "Company");

            if (result.userId > 0 && string.IsNullOrEmpty(result.errorMessage))
            {
                return RedirectToAction("SuccessPage", new { id = result.userId });
            }

            ViewData["registerErrors"] = result.errorMessage;
            return View(createCompany);
        }
    }
}
