using AQS_Application.Dtos.BaseServiceDto.BlogDtos;
using AQS_Common.Enums;
using Domin.Entities;

namespace AQS_Application.Interfaces.IServices.BaseServices
{
    public interface IBlogService
    {
        Task<int> Create(Blog blog);
        Task<ResultOutPutMethodEnum> Delete(int id);
        Task<ResultOutPutMethodEnum> DeleteBlogPicture(int pictureId);
        Task<List<BlogReadAdminDto>> Read();
        Task<List<BlogReadHomeDto>> ReadTop3();
        /// <summary>
        /// منسوخ کنید
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Blog?> ReadById(int id);
        Task<BlogReadAdminDto?> ReadByIdAsync(int id);
        Task<ResultOutPutMethodEnum> Update(Blog blog);
        Task<ResultOutPutMethodEnum> UpdateHeaderPicture(int id, string headerPictureFileName);
        Task<ResultOutPutMethodEnum> UpdateVideoFile(int id, string videoFileName);
        Task<ResultOutPutMethodEnum> UpdatePictureRout(int blogId, string rout);
    }

}