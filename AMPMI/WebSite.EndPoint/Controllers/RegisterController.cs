using AQS_Aplication.Interfaces.IServisces.IThirdParitesServices;
using AQS_Aplication.Services;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Models.AcountingViewModel.Register;

namespace WebSite.EndPoint.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ISMSOTPService _sMSOTPService;
        static int otp; // TODO : باید ساختار نهگداری otp به MemoryCatch تبدیل شود 
        public RegisterController(ISMSOTPService sMSOTPService)
        {
            this._sMSOTPService = sMSOTPService;
        }
        /// <summary>
        /// ورود به صفحه ثبت نام 
        /// </summary>
        /// <returns></returns>
        public IActionResult MoblieInput()
        {
            // phone : textbox : View
            return View();
        }
        [HttpPost]
        public IActionResult MoblieItput(string mobile) 
        {
            if (string.IsNullOrEmpty(mobile))
                return View();
            else
                return RedirectToAction(nameof(ConfirmOTP),new { mobile } );
        }
        /// <summary>
        /// ارسال OTP به شماره تلفن وارد شده
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public IActionResult ConfirmOTP(string mobile,string errorMsg="")
        {
            /// جنریت کردن otp
            if (string.IsNullOrEmpty(mobile))
                return RedirectToAction(nameof(MoblieInput));
            ViewData["mobile"] = mobile;
            if (!string.IsNullOrEmpty(errorMsg))
            {
                ViewData["error"]=errorMsg;
                return View();
            }

            GetOTP(true);

            //_sMSOTPService.SendSMSForAuthentication(mobile, GetOTP(true).ToString());
            
            return View();
        }
        [HttpPost]

        public IActionResult ConfirmOTP(string mobile, int userOtp)
        {

            ViewData["mobile"] = mobile;
            if (userOtp == GetOTP(false))
            {
                return RedirectToAction(nameof(Register), new { mobile });
            }
            else
            {
                return RedirectToAction(nameof(ConfirmOTP), new[] { mobile, "کد تایید اشتباه است" });
            }

        }
        public int GetOTP(bool generateNew)
        {
            if (otp > 100000 || !generateNew)
                return otp;
            else
            {
                otp = GenerateOTP();
                return otp;
            }
               
        }
        public int GenerateOTP()
        {
            Random random = new Random();
            int randomOtp = random.Next(100000, 1000000); // بازه شامل 100000 و کمتر از 1000000
            return randomOtp;
        }
        /// <summary>
        /// در صورت درست بودن شماره تلفن ، صفحه اصلی ثبت نام باز میشود . 
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="otp"></param>
        /// <returns></returns>
        public IActionResult Register(string mobile)
        {
            //ViewData["mobile"] = mobile;
            RegisterVM registerVM = new RegisterVM()
            {
                Mobile = mobile
            };
            return View(registerVM);
        }
        ///// <summary>
        ///// ثبت نام نهایی
        ///// </summary>
        ///// <returns></returns>
        [HttpPost]
        public IActionResult Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                if (registerVM.Password == registerVM.PasswordConfirm)
                {
                    // فرایند ثبت نام

                }
            }
            return View();
        }
    }
}
