using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Areas.Company.Controllers
{
    [Area("Company")]
    public class NotificationController : Controller
    {
        public IActionResult NotifList()
        {
            return View();
        }
    }
}
