using AQS_Aplication.Dtos.BaseServiceDto.CategoryDto;

namespace WebSite.EndPoint.Areas.Admin.Models.SubCategory
{
    public class SubCategoryVM
    {
        public int Id { get; set; }
        public int RowNum { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public List<CategoryReadDto> Categories { get; set; }
    }
}
