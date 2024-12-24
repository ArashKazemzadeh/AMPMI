using AQS_Common.Enums;
using Domin.Entities;

namespace AQS_Aplication.Interfaces.IServisces.BaseServices
{
    public interface ICategoryService
    {
        Task<int> Create(Category category);
        Task<ResultOutPutMethodEnum> Delete(int id);
        Task<List<Category>> Read();
        Task<Category?> ReadById(int id);
        Task<ResultOutPutMethodEnum> Update(Category category);
        Task<ResultOutPutMethodEnum> UpdatePicture(int id, Guid pictureFileName);
    }
}
