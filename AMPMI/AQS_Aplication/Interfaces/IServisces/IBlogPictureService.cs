using Domin.Entities;

namespace AQS_Aplication.Interfaces.IServisces
{
    public interface IBlogPictureService
    {
        Task<bool> Create(BlogPicture blog);
        Task Delete(int id);
        Task<List<BlogPicture>> Read();
        Task<BlogPicture> ReadById(int id);
        Task<BlogPicture> Update(BlogPicture blog);
    }

}