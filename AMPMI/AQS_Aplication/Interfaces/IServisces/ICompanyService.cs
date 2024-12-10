using Domin.Entities;
using AQS_Common.Enums;
using AQS_Aplication.Dtos;

namespace AQS_Aplication.Interfaces.IServisces
{
    public interface ICompanyService
    {
        Task<long> Create(RegisterIdentityDTO company, long id);
        Task<ResultServiceMethods> Delete(long id);
        Task<List<Company>> Read();
        Task<Company?> ReadById(long id);
        Task<ResultServiceMethods> Update(Company company);
        Task<ResultServiceMethods> IsCompany(long id, bool isCompany);
        Task<bool> IsExistByMobileNumber(string mobile);
    }
}
