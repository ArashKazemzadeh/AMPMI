using Domin.Entities;
using WebSite.EndPoint.Models.ProductViewModel;

namespace WebSite.EndPoint.Models.CompanyViewModel
{
    public class CompanyDetailVM
    {
        public string Name { get; set; } = null!;
        public string MobileNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public Guid? LogoRout { get; set; }
        public string? TeaserGuid { get; set; }
        public string? About { get; set; }
        public List<ProductVM>? Products { get; set; }
        public List<Guid>? CompanyPictures { get; set; }

    }
}
