using Domin.Entities;
using WebSite.EndPoint.Models.ProductViewModel;

namespace WebSite.EndPoint.Models.CompanyViewModel
{
    public class CompanyDetailVM
    {
        public string Name { get; set; } = null!;
        public string MobileNumber { get; set; } = null!;
        public string ManagerName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; } = null!;
        public string? LogoRout { get; set; }
        public string? Tel { get; set; }
        public string? TeaserGuid { get; set; }
        public string? About { get; set; }
        public List<ProductVM>? Products { get; set; }
        public List<string>? CompanyPictures { get; set; }

    }
}
