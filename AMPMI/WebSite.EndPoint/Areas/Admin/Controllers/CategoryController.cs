using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        public IActionResult CategoryList()
        {
            return View();
        }
        public IActionResult EditCategory(int id)
        {
            return View();
        }
        public IActionResult DeleteCategory(int id)
        {
            return View();
        }
    }
}
