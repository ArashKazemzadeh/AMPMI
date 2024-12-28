using AQS_Aplication.Dtos.BaseServiceDto.NotificationDtos;
using AQS_Aplication.Interfaces.IServisces.BaseServices;
using AQS_Aplication.Services;
using AQS_Domin.Entities.Acounting;
using Domin.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebSite.EndPoint.Areas.Company.Models;

namespace WebSite.EndPoint.Areas.Company.Controllers
{
    [Area("Company")]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly ISeenNotifByCompanyService _seenNotifByCompanyService;
        public NotificationController(INotificationService notificationService,
            ISeenNotifByCompanyService seenNotifByCompanyService)
        {
            this._notificationService = notificationService;  
            this._seenNotifByCompanyService = seenNotifByCompanyService;
        }
        public async Task<IActionResult> NotifList()
        {
            int companyId = 9; // just for test
            List<NotificationReadAdminDto> allData = await _notificationService.ReadAll();
            List<SeenNotifByCompany> seenData = await _seenNotifByCompanyService.ReadByCompanyId(companyId);
            List<NotifVM> notifList = allData.Select(x => new NotifVM() 
            {
                Id = x.Id,
                Subject = x.Subject,
                Description = x.Description,
                CreateAt = x.CreateAt.ToPersianDate(),
                IsSeen = seenData.Exists(x=>x.NotificationId == x.Id)
            }).ToList();

            if (notifList.Count == 0)
                return View(NotifVM.Seed());

            return View(notifList);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SeenNotif(int notifId)
        {
            int companyId = 9;
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
