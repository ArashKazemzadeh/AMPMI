using AQS_Aplication.Interfaces.IInfrastructure.IContext;
using AQS_Aplication.Interfaces.IServisces.BaseServices;
using AQS_Common.Enums;
using AQS_Domin.Entities;
using Microsoft.EntityFrameworkCore;

namespace AQS_Aplication.Services
{
    public class BannerService : IBannerService
    {
        private readonly IDbAmpmiContext _context;

        public BannerService(IDbAmpmiContext context)
        {
            _context = context;
        }

        public async Task<Guid> Create(Guid banner)
        {
            var newBanner = new Banner
            {
                Id = banner 
            };

            _context.Banners.Add(newBanner);

            int result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return newBanner.Id;
            }

            return Guid.Empty; 
        }

        public async Task<ResultOutPutMethodEnum> Delete(Guid id)
        {
            var banner = await _context.Banners.FindAsync(id);

            if (banner != null)
            {
                _context.Banners.Remove(banner);
                return await _context.SaveChangesAsync() > 0
                    ? ResultOutPutMethodEnum.savechanged
                    : ResultOutPutMethodEnum.dontSaved;
            }

            return ResultOutPutMethodEnum.recordNotFounded; 
        }

        public async Task<List<Guid>> ReadAll()
        {
            var result = await _context.Banners.Select(b => b.Id).ToListAsync();
            return result ?? new List<Guid>(); 
        }

        public async Task<Guid?> ReadById(Guid id)
        {
            var banner = await _context.Banners.FindAsync(id);
            return banner?.Id; 
        }
    }
}
