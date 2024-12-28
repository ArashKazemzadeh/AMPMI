using AQS_Common.Enums;
using AQS_Domin.Entities;

namespace AQS_Application.Interfaces.IServices.BaseServices
{
    public interface IBannerService
    {
        /// <summary>
        /// ایجاد یک بنر جدید
        /// </summary>
        /// <param name="banner">مدل بنر</param>
        /// <returns>شناسه بنر جدید</returns>
        Task<Guid> Create(Guid banner);

        /// <summary>
        /// حذف بنر
        /// </summary>
        /// <param name="id">شناسه بنر</param>
        /// <returns>وضعیت عملیات حذف</returns>
        Task<ResultOutPutMethodEnum> Delete(Guid id);

        /// <summary>
        /// دریافت همه بنرها
        /// </summary>
        /// <returns>لیست بنرها</returns>
        Task<List<Guid>> ReadAll();

        /// <summary>
        /// دریافت یک بنر بر اساس شناسه
        /// </summary>
        /// <param name="id">شناسه بنر</param>
        /// <returns>مدل بنر</returns>
        Task<Guid?> ReadById(Guid id);
    }
}