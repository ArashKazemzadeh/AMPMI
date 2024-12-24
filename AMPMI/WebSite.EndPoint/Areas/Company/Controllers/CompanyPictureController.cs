using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Areas.Company.Controllers
{
    [Area("Company")]
    public class CompanyPictureController : Controller
    {
        public IActionResult EditCompanyPicture()
        {
            return View();
        }
    }
}
