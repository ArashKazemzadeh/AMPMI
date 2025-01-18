using AQS_Application.Interfaces.IInfrastructure.IContext;
using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Common.Enums;
using Domin.Entities;
using Microsoft.EntityFrameworkCore;

namespace AQS_Application.Services
{
    public class SeenNotifByCompanyService : ISeenNotifByCompanyService
    {
        private readonly IDbAmpmiContext _context;

        public SeenNotifByCompanyService(IDbAmpmiContext context)
        {
            _context = context;
        }

        public async Task<long> Create(long notifId,long companyId)
        {
            var seenNotifByCompany = new SeenNotifByCompany() 
            {
                NotificationId = notifId,
                CompanyId = companyId
            };
            var row = _context.SeenNotifByCompanies.Add(seenNotifByCompany);

            int result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return row.Entity.Id; 
            }
            return -1;
        }

        public async Task<ResultOutPutMethodEnum> Delete(long id)
        {
            var record = await _context.SeenNotifByCompanies.FindAsync(id);
            if (record != null)
            {
                _context.SeenNotifByCompanies.Remove(record);
                return await _context.SaveChangesAsync() > 0
                    ? ResultOutPutMethodEnum.savechanged
                    : ResultOutPutMethodEnum.dontSaved;
            }
            return ResultOutPutMethodEnum.recordNotFounded;
        }

        public async Task<List<SeenNotifByCompany>> ReadAll()
        {
            var result = await _context.SeenNotifByCompanies
                .Include(s => s.Company)
                .Include(s => s.Notification)
                .ToListAsync();

            return result ?? new List<SeenNotifByCompany>();
        }

        public async Task<SeenNotifByCompany?> ReadById(long id)
        {
            return await _context.SeenNotifByCompanies
                .Include(s => s.Company)
                .Include(s => s.Notification)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<List<SeenNotifByCompany>> ReadByCompanyId(long companyId)
        {
            return await _context.SeenNotifByCompanies
                .Where(x => x.CompanyId == companyId)
                .ToListAsync();
        }
        public async Task<List<SeenNotifByCompany>> ReadByNotifId(long notifId)
        {
            return await _context.SeenNotifByCompanies
                .Where(x => x.NotificationId == notifId)
                .ToListAsync();
        }
        public async Task<bool> NotifIsSeenByCompany(long notifId,long companyId)
        {
            return await _context.SeenNotifByCompanies.AnyAsync(x => x.NotificationId == notifId &&
            x.CompanyId == companyId);
        }
    }
}
