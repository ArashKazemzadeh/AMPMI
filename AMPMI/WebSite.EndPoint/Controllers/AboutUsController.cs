using AQS_Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Controllers
{
    public class AboutUsController : Controller
    {
        private readonly IMiscellaneousDataService _miscellaneousDataService;

        public AboutUsController(IMiscellaneousDataService miscellaneousDataService)
        {
            _miscellaneousDataService = miscellaneousDataService;
        }
        public async Task<IActionResult> Index() //OK
        {
            var content = await _miscellaneousDataService.GetValueAsync("AboutUs");
            ViewBag.AboutUsContent = content ?? "محتوایی برای نمایش وجود ندارد.";
            return View();
        }
    }
}
