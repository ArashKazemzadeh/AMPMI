using AQS_Application.Dtos.BaseServiceDto.BlogDtos;
using AQS_Common.Enums;
using Domin.Entities;

namespace AQS_Application.Interfaces.IServices.BaseServices
{
    public interface IBlogService
    {
        Task<int> Create(Blog blog);
        Task<ResultOutPutMethodEnum> Delete(int id);
        Task<List<Blog>> Read();
        Task<List<BlogReadHomeDto>> ReadTop3();
        Task<Blog?> ReadById(int id);
        Task<ResultOutPutMethodEnum> Update(Blog blog);
        Task<ResultOutPutMethodEnum> UpdateHeaderPicture(int id, string headerPictureFileName);
        Task<ResultOutPutMethodEnum> UpdateVideoFile(int id, string videoFileName);
    }

}