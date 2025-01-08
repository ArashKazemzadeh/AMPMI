using AQS_Aplication.Dtos.BaseServiceDto.CategoryDto;
using AQS_Application.Dtos.BaseServiceDto.BlogDtos;
using AQS_Application.Dtos.BaseServiceDto.Company;
using AQS_Domin.Entities;

namespace WebSite.EndPoint.Models.HomeViewModel
{
    public class HomeVM
    {
        public List<CategoryReadDto> Categories { get; set; }
        public List<CompanyReadDto> Companies { get; set; }
        public List<BlogReadHomeDto> Blogs { get; set; }
        public List<Banner> Banners { get; set; }
    }
}
