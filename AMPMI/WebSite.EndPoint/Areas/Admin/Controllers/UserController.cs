using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        public IActionResult UserList()
        {
            return View();
        }
        public IActionResult EditUser()
        {
            return View();
        }
    }
}
