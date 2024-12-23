using Domin.Entities;
using AQS_Common.Enums;
using AQS_Aplication.Dtos.IdentityServiceDto;

namespace AQS_Aplication.Interfaces.IServisces.BaseServices
{
    public interface ICompanyService
    {
        Task<long> Create(RegisterIdentityDTO company, long id);
        Task<ResultOutPutMethodEnum> Delete(long id);
        Task<List<Company>> Read();
        Task<Company?> ReadById(long id);
        Task<ResultOutPutMethodEnum> Update(Company company);
        Task<ResultOutPutMethodEnum> IsCompany(long id, bool isCompany);
        Task<bool> IsExistByMobileNumber(string mobile);
        Task<bool> IsExistById(long id);
    }
}
