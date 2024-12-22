using AQS_Common.Enums;
using Domin.Entities;

namespace AQS_Aplication.Interfaces.IServisces.BaseServices
{
    public interface ISubCategoryService
    {
        /// <summary>
        /// ایجاد یک زیر دسته‌بندی جدید
        /// </summary>
        /// <param name="subCategory">مدل زیر دسته‌بندی</param>
        /// <returns>شناسه زیر دسته‌بندی جدید</returns>
        Task<int> Create(SubCategory subCategory);

        /// <summary>
        /// حذف یک زیر دسته‌بندی
        /// </summary>
        /// <param name="id">شناسه زیر دسته‌بندی</param>
        /// <returns>وضعیت عملیات حذف</returns>
        Task<ResultServiceMethods> Delete(int id);

        /// <summary>
        /// دریافت تمام زیر دسته‌بندی‌ها
        /// </summary>
        /// <returns>لیست زیر دسته‌بندی‌ها</returns>
        Task<List<SubCategory>> ReadAll();

        /// <summary>
        /// دریافت یک زیر دسته‌بندی بر اساس شناسه
        /// </summary>
        /// <param name="id">شناسه زیر دسته‌بندی</param>
        /// <returns>مدل زیر دسته‌بندی</returns>
        Task<SubCategory?> ReadById(int id);

        /// <summary>
        /// به‌روزرسانی اطلاعات زیر دسته‌بندی
        /// </summary>
        /// <param name="subCategory">مدل زیر دسته‌بندی</param>
        /// <returns>وضعیت عملیات به‌روزرسانی</returns>
        Task<ResultServiceMethods> Update(SubCategory subCategory);
    }
}