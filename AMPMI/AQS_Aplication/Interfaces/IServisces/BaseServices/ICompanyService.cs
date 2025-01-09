using Domin.Entities;
using AQS_Common.Enums;
using AQS_Application.Dtos.IdentityServiceDto;
using AQS_Application.Dtos.BaseServiceDto.Company;

namespace AQS_Application.Interfaces.IServices.BaseServices
{
    public interface ICompanyService
    {
        Task<long> Create(RegisterIdentityDTO company, long id);
        Task<ResultOutPutMethodEnum> Delete(long id);
        Task<List<Company>> Read();
        Task<CompanyEditProfileDto?> ReadByIdAsync(long id);
        /// <summary>
        /// دیتای کمپانی را به همراه محصولات و تصاویر کمپانی برمیگرداند
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CompanyEditProfileDto?> ReadByIdIncludePicturesAndProducts(long id);
        /// <summary>
        /// فقط تیزر را اپدیت میکند
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        Task<ResultOutPutMethodEnum> UpdateTeaser(CompanyEditProfileDto company);
        Task<ResultOutPutMethodEnum> UpdateEditProfile(CompanyEditProfileDto dto);
        Task<ResultOutPutMethodEnum> IsCompany(long id, bool isCompany);
        Task<bool> IsExistByMobileNumber(string mobile);
        Task<bool> IsExistById(long id);
        Task<Company?> ReadByMobileNumber(string mobile);
        /// <summary>
        /// ارسال درخواست به ادمین برای تبدیل شدن کاربر به شرکت
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sendRequest"></param>
        /// <returns></returns>
        Task<ResultOutPutMethodEnum> SendRequest(long id, bool sendRequest);
    }
}
