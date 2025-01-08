using AQS_Application.Dtos.BaseServiceDto.NotificationDtos;
using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Application.Interfaces.IServices.IdentityServices;
using Domin.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Areas.Company.Models;

namespace WebSite.EndPoint.Areas.Company.Controllers
{
    [Area("Company")]
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly ISeenNotifByCompanyService _seenNotifByCompanyService;
        private readonly ILoginService _loginService;

        public NotificationController(INotificationService notificationService,
            ISeenNotifByCompanyService seenNotifByCompanyService,ILoginService loginService)
        {
            this._notificationService = notificationService;  
            this._seenNotifByCompanyService = seenNotifByCompanyService;
            this._loginService = loginService;
        }
        public async Task<IActionResult> NotifList()
        {
            long companyId = await _loginService.GetUserIdAsync(User);
            List<NotificationReadAdminDto> allData = await _notificationService.ReadAll();
            List<SeenNotifByCompany> seenData = await _seenNotifByCompanyService.ReadByCompanyId(companyId);
            List<NotifVM> notifList = allData.Select(x => new NotifVM() 
            {
                Id = x.Id,
                Subject = x.Subject,
                Description = x.Description,
                CreateAt = x.CreateAt.ToPersianDate(),
                IsSeen = seenData.Any(m => m.NotificationId == x.Id)
            }).ToList();

            return View(notifList);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SeenNotif(int notifId)
        {
            long companyId = await _loginService.GetUserIdAsync(User);
            try
            {
                if (!await _seenNotifByCompanyService.NotifIsSeenByCompany(notifId, companyId))
                {
                    await _seenNotifByCompanyService.Create(notifId, companyId);
                }
                return Ok();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
