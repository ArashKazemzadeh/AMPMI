using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Areas.Company.Controllers
{
    [Area("Company")]
    public class CompanyProfileController : Controller
    {
        public IActionResult EditCompanyProfile()
        {
            return View();
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult ChangePassword(string password)
        //{
        //    return View();
        //}
    }
}
