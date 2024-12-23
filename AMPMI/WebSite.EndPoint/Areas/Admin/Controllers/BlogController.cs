using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        public IActionResult BlogList()
        {
            return View();
        }
        public IActionResult EditBlog()
        {
            return View();
        }
    }
}
