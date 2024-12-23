using AQS_Aplication.Dtos.BaseServiceDto.NotificationDtos;
using AQS_Common.Enums;
using Domin.Entities;

namespace AQS_Aplication.Interfaces.IServisces.BaseServices
{
    public interface INotificationService
    {
        /// <summary>
        /// ایجاد یک اعلان جدید
        /// </summary>
        /// <param name="subject">موضوع اعلان</param>
        /// <param name="description">توضیحات اعلان</param>
        /// <returns>شناسه اعلان ایجاد شده</returns>
        Task<long> Create(string subject, string description);

        /// <summary>
        /// حذف یک اعلان
        /// </summary>
        /// <param name="id">شناسه اعلان</param>
        /// <returns>وضعیت عملیات حذف</returns>
        Task<ResultOutPutMethodEnum> Delete(long id);

        /// <summary>
        /// به‌روزرسانی یک اعلان
        /// </summary>
        /// <param name="subject">موضوع اعلان</param>
        /// <param name="description">توضیحات اعلان</param>
        /// <returns>وضعیت عملیات به‌روزرسانی</returns>
        Task<ResultOutPutMethodEnum> Update(long id, string subject, string description);

        /// <summary>
        /// دریافت تمام اعلان‌های یک سال اخیر برای ادمین سایت
        /// </summary>
        /// <returns>لیست اعلان‌ها</returns>
        Task<List<NotificationReadAdminDto>> ReadAll();

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
        Task<ResultOutPutMethodEnum> AddSeenByCompany(long notificationId, long companyId);
    }

}
