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
        public IActionResult NotConfirmedProductList()
        {
            return View();
        }
        /// <summary>
        /// تایید محصول
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IActionResult ConfirmProduct(int productId)
        {
            return RedirectToAction(nameof(NotConfirmedProductList));
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
