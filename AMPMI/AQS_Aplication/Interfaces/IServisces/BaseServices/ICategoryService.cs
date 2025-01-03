using AQS_Aplication.Dtos.BaseServiceDto.CategoryDto;
using AQS_Common.Enums;
using Domin.Entities;

namespace AQS_Application.Interfaces.IServices.BaseServices
{
    public interface ICategoryService
    {
        Task<int> Create(string name, string img);
        Task<ResultOutPutMethodEnum> Delete(int id);
        Task<List<CategoryReadDto>> ReadAll();
        Task<Category?> ReadById(int id);
        Task<ResultOutPutMethodEnum> Update(int id, string name);
        Task<ResultOutPutMethodEnum> UpdatePicture(int id, string pictureFileName);
    }
}
