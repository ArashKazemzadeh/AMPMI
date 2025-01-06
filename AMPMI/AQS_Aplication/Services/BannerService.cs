using AQS_Application.Interfaces.IInfrastructure.IContext;
using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Common.Enums;
using AQS_Domin.Entities;
using Microsoft.EntityFrameworkCore;

namespace AQS_Application.Services
{
    public class BannerService : IBannerService
    {
        private readonly IDbAmpmiContext _context;

        public BannerService(IDbAmpmiContext context)
        {
            _context = context;
        }

        // ایجاد بنر جدید
        public async Task<bool> Create(BannerIdEnum bannerId, string rout)
        {
            var newBanner = new Banner
            {
                Id = bannerId,
                Rout = rout
            };

            await _context.Banners.AddAsync(newBanner);

            int result = await _context.SaveChangesAsync();

            return result > 0 ? true : false;
        }

        // حذف بنر با استفاده از Id
        public async Task<ResultOutPutMethodEnum> Delete(BannerIdEnum bannerId)
        {
            var banner = await _context.Banners.FirstOrDefaultAsync(b => b.Id == bannerId);

            if (banner != null)
            {
                _context.Banners.Remove(banner);
                return await _context.SaveChangesAsync() > 0
                    ? ResultOutPutMethodEnum.savechanged
                    : ResultOutPutMethodEnum.dontSaved;
            }

            return ResultOutPutMethodEnum.recordNotFounded;
        }

        // خواندن همه بنرها
        public async Task<List<Banner>> ReadAll()
        {
            var result = await _context.Banners.ToListAsync();
            return result ?? new List<Banner>();
        }

        // خواندن بنر بر اساس Id
        public async Task<Banner?> ReadById(BannerIdEnum bannerId)
        {
            return await _context.Banners.FirstOrDefaultAsync(b => b.Id == bannerId);
        }

        // ویرایش بنر
        public async Task<bool> Update(BannerIdEnum bannerId, string newRout)
        {
            var banner = await _context.Banners.FirstOrDefaultAsync(b => b.Id == bannerId);

            if (banner == null)
            {
                banner = new Banner
                {
                    Id = bannerId,
                    Rout = newRout
                };
                _context.Banners.Add(banner);
            }
            else
            {
                banner.Rout = newRout;
            }
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
