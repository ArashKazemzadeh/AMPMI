using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult Index() //OK
        {
            return View();
        }
    }
}
