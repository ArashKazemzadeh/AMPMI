using AQS_Common.Enums;
using Domin.Entities;

namespace AQS_Aplication.Interfaces.IServisces.BaseServices
{
    public interface IProductService
    {
        Task<long> Create(Product product);
        Task<ResultOutPutMethodEnum> Delete(long id);
        Task<List<Product>> Read();
        Task<Product?> ReadById(long id);
        Task<List<Product>> ReadByCompanyId(long id);
        Task<ResultOutPutMethodEnum> Update(Product product);
        Task<ResultOutPutMethodEnum> IsConfirmed(long id, bool isConfirmed);
        Task<ResultOutPutMethodEnum> UpdatePictureFileName(int id, Guid pictureFileName);
    }
}
