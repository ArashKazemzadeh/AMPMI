using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string mobile)
        {
            return View();
        }
        public IActionResult MobileInput()
        {
            // phone : textbox : View
            return View();
        }
        public IActionResult ConfirmOTP(string mobile)
        {
            /// جنریت کردن otp
            if (string.IsNullOrEmpty(mobile))
                return RedirectToAction(nameof(Login));

            ViewData["mobile"] = mobile;

            var x = TempData["error"];

            return View();
        }
    }
}
