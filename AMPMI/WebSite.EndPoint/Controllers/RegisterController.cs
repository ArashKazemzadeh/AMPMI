using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Controllers
{
    public class RegisterController : Controller
    {

        /// <summary>
        /// ورود به صفحه ثبت نام 
        /// </summary>
        /// <returns></returns>
        public IActionResult MoblieInput()
        {
            // phone : textbox : View
            return View();
        }
        /// <summary>
        /// ارسال OTP به شماره تلفن وارد شده
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ConfirmOTP(string mobile)
        {
            /// جنریت کردن otp
            if (string.IsNullOrEmpty(mobile))
                return RedirectToAction(nameof(MoblieInput));

            ViewData["mobile"] = mobile;

            var x = TempData["error"];
            
            return View();
        }
        /// <summary>
        /// در صورت درست بودن شماره تلفن ، صفحه اصلی ثبت نام باز میشود . 
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="otp"></param>
        /// <returns></returns>
        public IActionResult Register(string mobile , int otp)
        {
            /// جمریت کردن otp
            // phone : textbox : View
            // otp : textbox : View
            ViewData["mobile"]=mobile;
            if (otp == 0) /*: is exist*/
            {
                return View();
            }
            else
            {
                TempData["error"]= "";
                return RedirectToAction(nameof(ConfirmOTP), mobile);
            }
            
        }
        /// <summary>
        /// ثبت نام نهایی
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Register()
        {
            return View();
        }
    }
}
