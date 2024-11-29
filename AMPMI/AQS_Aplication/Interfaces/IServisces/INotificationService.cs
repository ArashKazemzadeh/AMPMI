using AQS_Common.Enums;
using Domin.Entities;

namespace AQS_Aplication.Interfaces.IServisces
{
    public interface INotificationService
    {
        /// <summary>
        /// ایجاد یک اعلان جدید
        /// </summary>
        /// <param name="notification">مدل اعلان</param>
        /// <returns>شناسه اعلان ایجاد شده</returns>
        Task<long> Create(Notification notification);

        /// <summary>
        /// حذف یک اعلان
        /// </summary>
        /// <param name="id">شناسه اعلان</param>
        /// <returns>وضعیت عملیات حذف</returns>
        Task<ResultServiceMethods> Delete(long id);

        /// <summary>
        /// به‌روزرسانی یک اعلان
        /// </summary>
        /// <param name="notification">مدل اعلان با تغییرات</param>
        /// <returns>وضعیت عملیات به‌روزرسانی</returns>
        Task<ResultServiceMethods> Update(Notification notification);

        /// <summary>
        /// دریافت تمام اعلان‌ها
        /// </summary>
        /// <returns>لیست اعلان‌ها</returns>
        Task<List<Notification>> ReadAll();

        /// <summary>
        /// دریافت یک اعلان با شناسه
        /// </summary>
        /// <param name="id">شناسه اعلان</param>
        /// <returns>اعلان مورد نظر</returns>
        Task<Notification?> ReadById(long id);

        /// <summary>
        /// اضافه کردن مشاهده توسط شرکت
        /// </summary>
        /// <param name="notificationId">شناسه اعلان</param>
        /// <param name="companyId">شناسه شرکت</param>
        /// <returns>وضعیت عملیات</returns>
        Task<ResultServiceMethods> AddSeenByCompany(long notificationId, long companyId);
    }

}
