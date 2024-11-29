using AQS_Common.Enums;
using Domin.Entities;

namespace AQS_Aplication.Interfaces.IServisces
{
    public interface ICategoryService
    {
        Task<int> Create(Category category);
        Task<ResultServiceMethods> Delete(int id);
        Task<List<Category>> Read();
        Task<Category?> ReadById(int id);
        Task<ResultServiceMethods> Update(Category category);
        Task<ResultServiceMethods> UpdatePicture(int id, Guid pictureFileName);
    }
}
