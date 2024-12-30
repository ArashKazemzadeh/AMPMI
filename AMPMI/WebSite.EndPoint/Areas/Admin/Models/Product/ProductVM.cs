using Domin.Entities;
using System.ComponentModel.DataAnnotations;

namespace WebSite.EndPoint.Areas.Admin.Models.Product
{
    public class ProductVM
    {
        public long Id { get; set; }
        public int RowNum { get; set; }
        [Required(ErrorMessage = "نام محصول نمیتواند خالی باشد")]
        public string Name { get; set; } 

        public string Description { get; set; } = null!;

        public IFormFile PictureFileName { get; set; }
        public string PictureFileSrc { get; set; }
        public bool IsPictureChanged { get; set; }

        public bool IsConfirmed { get; set; }

        public long? CompanyId { get; set; }
        [Required(ErrorMessage = "گروه فرعی انتخاب نشده است")]
        [Range(0.1,double.MaxValue,ErrorMessage ="گروه فرعی انتخاب نشده است")]
        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }
        //public List<SubCategory> SubCategories { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
