using AQS_Application.Dtos.BaseServiceDto.Company;
using Domin.Entities;

namespace WebSite.EndPoint.Areas.Admin.Models.Company
{
    public class CompanyVM
    {
        //public string   Password { get; set; } = null!;
        public long Id { get; set; }
        public string Name { get; set; }
        public string ManagerName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string? Brands { get; set; }
        public int Capacity { get; set; }
        public string? Partnership { get; set; }
        public string? QualityGrade { get; set; }
        public string? Iso { get; set; }
        public string? About { get; set; }
        public string? LogoRout { get; set; }
        public IFormFile? Logo { get; set; }
        public string? Tel { get; set; }
        public bool IsLogoChanged { get; set; }
        public bool IsCompany { get; set; }
        public bool SendRequst { get; set; }
        public string? TeaserGuid { get; set; }

        //public virtual ICollection<CompanyPicture> CompanyPictures { get; set; } = new List<CompanyPicture>();

        //public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        //public virtual ICollection<SeenNotifByCompany> SeenNotifByCompanies { get; set; } = new List<SeenNotifByCompany>();


        public CompanyEditProfileDto MapToDto(CompanyVM company)
        {
            return new CompanyEditProfileDto
            {
                Id = company.Id,
                Name = company.Name,
                ManagerName = company.ManagerName,
                MobileNumber = company.MobileNumber,
                Email = company.Email,
                Address = company.Address,
                Brands = company.Brands,
                Capacity = company.Capacity,
                Partnership = company.Partnership,
                QualityGrade = company.QualityGrade,
                Iso = company.Iso,
                About = company.About,
                LogoRout = company.LogoRout == null ? string.Empty : company.LogoRout,
                IsCompany = company.IsCompany,
                Tel = company.Tel,
            };
        }
    }
}
