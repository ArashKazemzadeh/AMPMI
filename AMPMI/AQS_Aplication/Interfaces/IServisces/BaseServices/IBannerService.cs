using AQS_Common.Enums;
using AQS_Domin.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AQS_Application.Interfaces.IServices.BaseServices
{
    public interface IBannerService
    {
        /// <summary>
        /// ایجاد یک بنر جدید
        /// </summary>
        Task<bool> Create(BannerIdEnum bannerId, string rout);

        /// <summary>
        /// حذف یک بنر بر اساس Id
        /// </summary>
        Task<ResultOutPutMethodEnum> Delete(BannerIdEnum bannerId);

        /// <summary>
        /// خواندن همه بنرها
        /// </summary>
        Task<List<Banner>> ReadAll();

        /// <summary>
        /// خواندن یک بنر بر اساس Id
        /// </summary>
        Task<Banner?> ReadById(BannerIdEnum bannerId);

        /// <summary>
        /// ویرایش یک بنر بر اساس Id
        /// </summary>
        Task<bool> Update(BannerIdEnum bannerId, string newRout, BannerTypeEnum type);
    }
}
