using AQS_Common.Enums;
using Domin.Entities;

namespace AQS_Application.Interfaces.IServices.BaseServices
{
    public interface ISeenNotifByCompanyService
    {
        /// <summary>
        /// اضافه کردن یک رکورد جدید مشاهده اعلان توسط شرکت
        /// </summary>
        /// <param name="notifId">شناسه اعلان</param>
        /// <param name="companyId">شناسه کمپانی</param>
        /// <returns>شناسه رکورد جدید</returns>
        Task<long> Create(int notifId, long companyId);

        /// <summary>
        /// حذف رکورد مشاهده اعلان توسط شرکت
        /// </summary>
        /// <param name="id">شناسه رکورد</param>
        /// <returns>وضعیت عملیات حذف</returns>
        Task<ResultOutPutMethodEnum> Delete(long id);

        /// <summary>
        /// دریافت تمام رکوردهای مشاهده اعلان توسط شرکت
        /// </summary>
        /// <returns>لیست رکوردها</returns>
        Task<List<SeenNotifByCompany>> ReadAll();

        /// <summary>
        /// دریافت یک رکورد مشاهده اعلان توسط شرکت با شناسه
        /// </summary>
        /// <param name="id">شناسه رکورد</param>
        /// <returns>رکورد مورد نظر</returns>
        Task<SeenNotifByCompany?> ReadById(long id);
        /// <summary>
        /// دریافت لیست اعلان های مشاهده شده توسط یک کمپانی
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task<List<SeenNotifByCompany>> ReadByCompanyId(long companyId);
        /// <summary>
        /// بررسی اینکه ایا یک اعلان توسط یک کمپانی مشاهده شده است یا خیر
        /// </summary>
        /// <param name="notifId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task<bool> NotifIsSeenByCompany(int notifId, long companyId);
    }
}
