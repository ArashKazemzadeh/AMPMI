using AQS_Common.Enums;
using AQS_Domin.Entities.business;

namespace AQS_Aplication.Interfaces.IServisces.BaseServices
{
    public interface IBlogService
    {
        Task<int> Create(Blog blog);
        Task<ResultOutPutMethodEnum> Delete(int id);
        Task<List<Blog>> Read();
        Task<Blog?> ReadById(int id);
        Task<ResultOutPutMethodEnum> Update(Blog blog);
        Task<ResultOutPutMethodEnum> UpdateHeaderPicture(int id, Guid headerPictureFileName);
        Task<ResultOutPutMethodEnum> UpdateVideoFile(int id, Guid videoFileName);
    }

}