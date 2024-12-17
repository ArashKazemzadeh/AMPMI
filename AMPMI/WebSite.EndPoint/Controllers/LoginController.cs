using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Controllers
{
    public class LoginController : Controller //ToDo : Arash
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
    }
}
