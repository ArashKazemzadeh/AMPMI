using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Controllers
{
    public class LoginController : Controller
    {
        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string mobmail)
        {
            if (ModelState.IsValid)
            {
                return View();
            }
            return NotFound();
        }
    }
}
