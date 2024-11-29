using AQS_Common.Enums;
using Domin.Entities;

namespace AQS_Aplication.Interfaces.IServisces
{
    public interface IBlogService
    {
        Task<int> Create(Blog blog);
        Task<ResultServiceMethods> Delete(int id);
        Task<List<Blog>> Read();
        Task<Blog?> ReadById(int id);
        Task<ResultServiceMethods> Update(Blog blog);
        Task<ResultServiceMethods> UpdateHeaderPicture(int id, Guid headerPictureFileName);
        Task<ResultServiceMethods> UpdateVideoFile(int id, Guid videoFileName);
    }

}