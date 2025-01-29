using AQS_Application.Services;
using AQS_Domin.Entities;
using AQS_Persistence.Contexts.SqlServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebSite.EndPoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AboutUsController : Controller
    {
        private readonly IMiscellaneousDataService _miscellaneousDataService;

        public AboutUsController(IMiscellaneousDataService miscellaneousDataService)
        {
            _miscellaneousDataService = miscellaneousDataService;
        }
        
        public async Task<IActionResult> ShowAboutUs()
        {
            var content = await _miscellaneousDataService.GetValueAsync("AboutUs");
            ViewBag.AboutUsContent = content ?? string.Empty;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAboutUs(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["Message"] = "محتوا نمی‌تواند خالی باشد!";
                return RedirectToAction("ShowAboutUs");
            }

            var result = await _miscellaneousDataService.SaveValueAsync("AboutUs", content);

            if (result)
            {
                TempData["Message"] = "محتوا با موفقیت ذخیره شد.";
            }
            else
            {
                TempData["Message"] = "خطایی در ذخیره‌سازی رخ داد.";
            }

            return RedirectToAction("ShowAboutUs");
        }
    }

}

