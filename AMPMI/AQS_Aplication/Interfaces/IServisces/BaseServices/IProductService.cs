using AQS_Common.Enums;
using Domin.Entities;

namespace AQS_Application.Interfaces.IServices.BaseServices
{
    public interface IProductService
    {
        Task<long> Create(Product product);
        Task<ResultOutPutMethodEnum> Delete(long id);
        Task<List<Product>> Read();
        Task<List<Product>> ReadNotConfirmed();
        Task<Product?> ReadById(long id);
        /// <summary>
        /// جستوجوی محصول براساس نام
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<List<Product>> SearchProductByName(string name);
        /// <summary>
        /// جستجوی محصول براساس نام و گروه اصلی
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<Product>> SearchProductByNameAndCategory(string name,int categoryId);
        /// <summary>
        /// جزیات محصول به همراه گروه اصلی ، گروه فرعی و کمپانی
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Product?> ReadByIdIncludeCategoryAndSubCategoryAndCompany(long id);
        Task<List<Product>> ReadByCategoryId(int categoryId);
        Task<List<Product>> ReadByCompanyId(long id);
        Task<ResultOutPutMethodEnum> Update(Product product);
        Task<ResultOutPutMethodEnum> IsConfirmed(long id, bool isConfirmed);
        Task<ResultOutPutMethodEnum> UpdatePictureFileName(int id, string pictureFileName);
    }
}
