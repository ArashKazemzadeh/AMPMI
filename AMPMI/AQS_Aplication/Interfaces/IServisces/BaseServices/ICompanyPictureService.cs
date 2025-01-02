using AQS_Common.Enums;
using Domin.Entities;

namespace AQS_Application.Interfaces.IServices.BaseServices
{
    public interface ICompanyPictureService
    {
        Task<long> Create(long companyId, string pictureFileName);
        Task<ResultOutPutMethodEnum> Delete(long id);
        Task<List<CompanyPicture>> ReadAll();
        Task<List<CompanyPicture>> ReadAllByCompany(long companyId);
        Task<CompanyPicture?> ReadById(long id);
        Task<ResultOutPutMethodEnum> Update(long id,long companyId, string pictureFileName);
    }
}