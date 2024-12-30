using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Common.Enums;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Areas.Admin.Models.Notification;

namespace WebSite.EndPoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> ShowPage()
        {
            var message = TempData["Message"]?.ToString();

            if (!string.IsNullOrEmpty(message))
                ViewData["Message"] = message;

            var Notifications = await _notificationService.ReadAll();
            var model = NotificationReadByAdminVM.ConvertToModel(Notifications);

            if (model.Count < 1)
                model = NotificationReadByAdminVM.Seed();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string subject, string description)
        {
            if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(description))
                return RedirectToAction(nameof(ShowPage));

            long id = await _notificationService.Create(subject, description);

            TempData["Message"] = id > 0 ? "اعلان با موفقیت ایجاد شد" : "اعلان جدید ایجاد نشد";

            return RedirectToAction(nameof(ShowPage));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            if (id < 1)
                return RedirectToAction(nameof(ShowPage));

            var resultMessage = await _notificationService.Delete(id);

            TempData["Message"] = resultMessage == ResultOutPutMethodEnum.savechanged ? "اعلان حذف شد" :
                                  resultMessage == ResultOutPutMethodEnum.recordNotFounded ? "اعلان یافت نشد" :
                                  "اعلان حذف نشد";

            return RedirectToAction(nameof(ShowPage));
        }
    }
}
