using AQS_Aplication.Interfaces.IServisces.BaseServices;
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
            this._notificationService = notificationService;
        }

        public async Task<IActionResult> ShowPage()
        {
            var Notifications = await _notificationService.ReadAll();
            var model = NotificationReadByAdminVM
                .ConvertToModel(Notifications);
            return View(model);
        }
    }
}
