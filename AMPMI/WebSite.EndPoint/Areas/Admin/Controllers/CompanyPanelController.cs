using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Areas.Admin.Controllers
{
    public class CompanyPanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
