using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Areas.Company.Controllers
{
    [Area("Company")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
