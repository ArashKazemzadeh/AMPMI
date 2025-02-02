using AQS_Aplication.Dtos.BaseServiceDto.CategoryDto;
using AQS_Application.Dtos.BaseServiceDto.CategoryDtos;
using Domin.Entities;
using System.ComponentModel.DataAnnotations;

namespace WebSite.EndPoint.Areas.Company.Models.Product
{
    public class ProductVM
    {
        public long Id { get; set; }
        public int RowNum { get; set; }
        [Required(ErrorMessage = "نام محصول نمیتواند خالی باشد")]
        public string Name { get; set; }

        public string? Description { get; set; } = "";

        public bool IsConfirmed { get; set; }

        public long? CompanyId { get; set; }
        [Required(ErrorMessage = "گروه فرعی انتخاب نشده است")]
        [Range(0.1,double.MaxValue,ErrorMessage ="گروه فرعی انتخاب نشده است")]
        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }
        /// <summary>
        /// برای آپلود عکس
        /// </summary>
        public List<IFormFile>? PictureFileName { get; set; }
        public List<ProductPicture>? Pictures { get; set; }
        public List<CategoryIncludeSubCategoriesDto>? Categories { get; set; }
    }
}
