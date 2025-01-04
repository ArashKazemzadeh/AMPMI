using AQS_Application.Dtos.BaseServiceDto.SubCategoryDto;
namespace AQS_Application.Dtos.BaseServiceDto.CategoryDtos;
public class CategoryIncludeSubCategoriesDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PictureFileName { get; set; }
    public List<SubCategoryReadDto> SubCategories { get; set; }
}

