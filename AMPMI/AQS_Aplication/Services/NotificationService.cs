using AQS_Aplication.Interfaces.Context;
using AQS_Aplication.Interfaces.IServisces;
using AQS_Common.Enums;
using Domin.Entities;
using Microsoft.EntityFrameworkCore;

namespace AQS_Aplication.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IDbAmpmiContext _context;

        public NotificationService(IDbAmpmiContext context)
        {
            _context = context;
        }

        // ایجاد اعلان جدید
        public async Task<long> Create(Notification notification)
        {
            notification.CreateAt = DateTime.UtcNow; 
            var row = _context.Notifications.Add(notification);
            int result = await _context.SaveChangesAsync();

            if (result > 0)
                return row.Entity.Id;
            return -1;
        }

        public async Task<ResultServiceMethods> Delete(long id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                return await _context.SaveChangesAsync() > 0
                    ? ResultServiceMethods.savechanged
                    : ResultServiceMethods.dontSaved;
            }
            return ResultServiceMethods.recordNotFounded;
        }

        public async Task<ResultServiceMethods> Update(Notification notification)
        {
            var existingNotification = await _context.Notifications.FindAsync(notification.Id);

            if (existingNotification == null)
                return ResultServiceMethods.recordNotFounded;

            if (!string.IsNullOrEmpty(notification.Subject) && existingNotification.Subject != notification.Subject)
                existingNotification.Subject = notification.Subject;

            if (!string.IsNullOrEmpty(notification.Description) && existingNotification.Description != notification.Description)
                existingNotification.Description = notification.Description;

            existingNotification.CreateAt = notification.CreateAt ?? existingNotification.CreateAt;

            return await _context.SaveChangesAsync() > 0
                ? ResultServiceMethods.savechanged
                : ResultServiceMethods.dontSaved;
        }

        public async Task<List<Notification>> ReadAll()
        {
            return await _context.Notifications.ToListAsync();
        }

        public async Task<Notification?> ReadById(long id)
        {
            return await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<ResultServiceMethods> AddSeenByCompany(long notificationId, long companyId)
        {
            var notification = await _context.Notifications
                .Include(n => n.SeenNotifByCompanies)
                .FirstOrDefaultAsync(n => n.Id == notificationId);

            if (notification == null)
                return ResultServiceMethods.recordNotFounded;

            if (!notification.SeenNotifByCompanies.Any(s => s.CompanyId == companyId))
            {
                notification.SeenNotifByCompanies.Add(new SeenNotifByCompany
                {
                    NotificationId = notificationId,
                    CompanyId = companyId
                });

                return await _context.SaveChangesAsync() > 0
                    ? ResultServiceMethods.savechanged
                    : ResultServiceMethods.dontSaved;
            }

            return ResultServiceMethods.duplicateRecord;
        }
    }
}
