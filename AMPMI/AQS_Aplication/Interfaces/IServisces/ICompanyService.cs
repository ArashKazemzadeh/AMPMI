using Domin.Entities;
using AQS_Common.Enums;

namespace AQS_Aplication.Interfaces.IServisces
{
    public interface ICompanyService
    {
        Task<long> Create(Company company);
        Task<ResultServiceMethods> Delete(long id);
        Task<List<Company>> Read();
        Task<Company?> ReadById(long id);
        Task<ResultServiceMethods> Update(Company company);
        Task<ResultServiceMethods> IsCompany(long id, bool isCompany);
    }
}
