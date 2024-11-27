using Domin.Entities;

namespace AQS_Aplication.Interfaces.IServisces
{
    public interface ICategoryService
    {
        Task<bool> Create(Category category);
        Task<Category> ReadById(int id);
        Task<List<Category>> Read();
        Task<Category> Update(Category category);
        Task Delete(int id);
    }
}
