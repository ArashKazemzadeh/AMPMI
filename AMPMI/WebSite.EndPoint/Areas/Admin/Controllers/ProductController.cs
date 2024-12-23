using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        public IActionResult ProductList()
        {
            return View();
        }
        public IActionResult EditProduct()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult NewProduct()
        //{
        //    return RedirectToAction(nameof(ProductList));
        //}
    }
}
