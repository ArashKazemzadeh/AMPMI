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
        /// <param name="isConfirmed">تایید شده</param>
        /// <returns></returns>
        Task<List<Product>> SearchProductByName(string name, bool isConfirmed);
        /// <summary>
        /// جستجوی محصول براساس نام و گروه اصلی 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="isConfirmed">تایید شده</param>
        /// <returns></returns>
        Task<List<Product>> SearchProductByNameAndCategory(string name,int categoryId, bool isConfirmed);
        /// <summary>
        /// جستجوی محصول بر اساس کمپانی و نام
        /// </summary>
        /// <param name="name"></param>
        /// <param name="companyId"></param>
        /// <param name="isConfirmed"></param>
        /// <returns></returns>
        Task<List<Product>> SearchByProductNameAndCompanyId(string name, long companyId, bool isConfirmed);
        /// <summary>
        /// جزیات محصول به همراه گروه اصلی ، گروه فرعی و کمپانی
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isConfirmed">تایید شده</param>
        /// <returns></returns>
        Task<Product?> ReadByIdIncludeCategoryAndSubCategoryAndCompany(long id, bool isConfirmed);
        /// <summary>
        /// محصولات گروه اصلی انتخاب شده را بر میگرداند
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="isConfirmed">تایید شده</param>
        /// <returns></returns>
        Task<List<Product>> ReadByCategoryId(int categoryId, bool isConfirmed);
        Task<List<Product>> ReadByCompanyId(long id);
        Task<ResultOutPutMethodEnum> Update(Product product);
        Task<ResultOutPutMethodEnum> IsConfirmed(long id, bool isConfirmed);
        Task<ResultOutPutMethodEnum> UpdatePictureRout(long productId, string rout);
        /// <summary>
        /// عکس های کالا
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<List<ProductPicture>> ReadPictureRouts(long productId);
        /// <summary>
        /// حذف عکس محصول
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        Task<ResultOutPutMethodEnum> DeleteProductPicture(long pictureId);
    }
}
