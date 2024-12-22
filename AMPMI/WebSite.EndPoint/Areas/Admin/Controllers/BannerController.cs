using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BannerController : Controller
    {
        public IActionResult BannerList()
        {
            return View();
        }
    }
}
