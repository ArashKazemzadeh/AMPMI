using Domin.Entities;

namespace AQS_Aplication.Interfaces.IServisces
{
    public interface IBlogService
    {
        Task<bool> Create(Blog blog);
        Task Delete(int id);
        Task<List<Blog>> Read();
        Task<Blog> ReadById(int id);
        Task<Blog> Update(Blog blog);
    }

}