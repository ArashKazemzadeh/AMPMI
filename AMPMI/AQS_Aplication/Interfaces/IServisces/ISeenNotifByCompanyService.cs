﻿using AQS_Common.Enums;
using Domin.Entities;

namespace AQS_Aplication.Interfaces.IServisces
{
    public interface ISeenNotifByCompanyService
    {
        /// <summary>
        /// اضافه کردن یک رکورد جدید مشاهده اعلان توسط شرکت
        /// </summary>
        /// <param name="seenNotifByCompany">مدل مشاهده اعلان توسط شرکت</param>
        /// <returns>شناسه رکورد جدید</returns>
        Task<long> Create(SeenNotifByCompany seenNotifByCompany);

        /// <summary>
        /// حذف رکورد مشاهده اعلان توسط شرکت
        /// </summary>
        /// <param name="id">شناسه رکورد</param>
        /// <returns>وضعیت عملیات حذف</returns>
        Task<ResultServiceMethods> Delete(long id);

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
    }
}
