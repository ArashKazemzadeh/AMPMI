using Domin.Entities;

namespace AQS_Aplication.Interfaces.IServisces
{
    public interface ISubCategoryService
    {
        Task<bool> Create(SubCategory subCategory);
        Task Delete(int id);
        Task<List<SubCategory>> Read();
        Task<SubCategory> ReadById(int id);
        Task<SubCategory> Update(SubCategory subCategory);
    }
}