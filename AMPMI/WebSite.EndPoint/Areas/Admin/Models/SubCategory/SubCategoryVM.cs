using AQS_Aplication.Dtos.BaseServiceDto.CategoryDto;
using System.ComponentModel.DataAnnotations;

namespace WebSite.EndPoint.Areas.Admin.Models.SubCategory
{
    public class SubCategoryVM
    {
        public int Id { get; set; }
        public int RowNum { get; set; }
        [Required(ErrorMessage ="نام گروه فرعی وارد نشده است")]
        public string Name { get; set; }
        [Required(ErrorMessage ="گروه اصلی انتخاب نشده است")]
        [Range(1,double.MaxValue,ErrorMessage ="گروه اصلی معتبر نیست")]
        public int CategoryId { get; set; }
        public List<CategoryReadDto> Categories { get; set; }
    }
}
