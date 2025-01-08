using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Areas.Company.Controllers
{
    [Area("Company")]
    [Authorize]
    public class CompanyPanelController : Controller
    {
        public IActionResult Panel()
        {
            return View();
        }
    }
}
