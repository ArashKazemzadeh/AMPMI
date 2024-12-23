using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoryController : Controller
    {
        public IActionResult SubCategoryList()
        {
            return View();
        }
    }
}
