using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Areas.Company.Controllers
{
    [Area("Company")]
    public class CompanyTeaserController : Controller
    {
        public IActionResult EditCompanyTeaser()
        {
            return View();
        }
    }
}
