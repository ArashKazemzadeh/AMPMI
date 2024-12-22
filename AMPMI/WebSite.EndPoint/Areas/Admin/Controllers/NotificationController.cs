using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NotificationController : Controller
    {
        public IActionResult ShowPage()
        {
            return View();
        }
    }
}
