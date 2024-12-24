using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Areas.Company.Controllers
{
    [Area("Company")]
    public class CompanyPanelController : Controller
    {
        public IActionResult Panel()
        {
            return View();
        }
    }
}
