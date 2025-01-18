using AQS_Application.Dtos.BaseServiceDto.NotificationDtos;
using AQS_Application.Interfaces.IInfrastructure.IContext;
using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Common.Enums;
using Domin.Entities;
using Microsoft.EntityFrameworkCore;

namespace AQS_Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IDbAmpmiContext _context;
        private readonly ISeenNotifByCompanyService _seenNotifByCompany;

        public NotificationService(IDbAmpmiContext context, ISeenNotifByCompanyService seenNotifByCompany)
        {
            _context = context;
            _seenNotifByCompany = seenNotifByCompany;
        }

        public async Task<long> Create(string subject, string description)
        {
            Notification notification = new()
            {
                CreateAt = DateTime.Now,
                Subject = subject,
                Description = description
            };
            var row = _context.Notifications.Add(notification);
            int result = await _context.SaveChangesAsync();

            if (result > 0)
                return row.Entity.Id;
            return -1;
        }

        public async Task<ResultOutPutMethodEnum> Delete(long id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            var seenNotif = await _seenNotifByCompany.ReadByNotifId(id);
            foreach(var item in seenNotif)
            {
                await _seenNotifByCompany.Delete(item.Id);
            }
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                return await _context.SaveChangesAsync() > 0
                    ? ResultOutPutMethodEnum.savechanged
                    : ResultOutPutMethodEnum.dontSaved;
            }
            return ResultOutPutMethodEnum.recordNotFounded;
        }

        public async Task<ResultOutPutMethodEnum> Update(long id, string subject, string description)
        {
            var existingNotification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);

            if (existingNotification == null)
                return ResultOutPutMethodEnum.recordNotFounded;

            existingNotification.Subject = subject;
            existingNotification.Description = description;
            existingNotification.CreateAt = DateTime.Now;

            return await _context.SaveChangesAsync() > 0
                ? ResultOutPutMethodEnum.savechanged
                : ResultOutPutMethodEnum.dontSaved;
        }

        public async Task<List<NotificationReadAdminDto>> ReadAll()
        {
            var notifications = await _context.Notifications
                .Where(n => n.CreateAt >= DateTime.UtcNow.AddDays(-366))
                .OrderByDescending(n => n.CreateAt)
                .Select(n => new NotificationReadAdminDto
                {
                    Id = n.Id,
                    Subject = n.Subject,
                    Description = n.Description,
                    CreateAt = n.CreateAt
                })
                .ToListAsync();

            return notifications == null ? new List<NotificationReadAdminDto>() : notifications;
        }
        public async Task<List<NotificationReadAdminDto>> ReadAllUnSeen(long companyId)
        {
            var allNotif = await ReadAll();
            var seenNotifsByCompay = await _seenNotifByCompany.ReadByCompanyId(companyId);
            List<NotificationReadAdminDto> result = new List<NotificationReadAdminDto>();
            foreach(var item in allNotif)
            {
                if(!seenNotifsByCompay.Any(x=>x.NotificationId == item.Id))
                {
                    result.Add(new NotificationReadAdminDto() 
                    { 
                        Id=item.Id,
                        Subject = item.Subject,
                        CreateAt=item.CreateAt,
                        Description = item.Description
                    });
                }
            }
            return result;
        }


        public async Task<Notification?> ReadById(long id)
        {
            return await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<ResultOutPutMethodEnum> AddSeenByCompany(long notificationId, long companyId)
        {
            var notification = await _context.Notifications
                .Include(n => n.SeenNotifByCompanies)
                .FirstOrDefaultAsync(n => n.Id == notificationId);

            if (notification == null)
                return ResultOutPutMethodEnum.recordNotFounded;

            if (!notification.SeenNotifByCompanies.Any(s => s.CompanyId == companyId))
            {
                notification.SeenNotifByCompanies.Add(new SeenNotifByCompany
                {
                    NotificationId = notificationId,
                    CompanyId = companyId
                });

                return await _context.SaveChangesAsync() > 0
                    ? ResultOutPutMethodEnum.savechanged
                    : ResultOutPutMethodEnum.dontSaved;
            }

            return ResultOutPutMethodEnum.duplicateRecord;
        }
    }
}
