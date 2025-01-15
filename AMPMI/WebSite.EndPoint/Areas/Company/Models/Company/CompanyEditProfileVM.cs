using AQS_Application.Dtos.BaseServiceDto.Company;

namespace WebSite.EndPoint.Areas.Company.Models.Company
{
    public class CompanyEditProfileVM
    {
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
        public string? LogoRout { get; set; } = string.Empty;
        public IFormFile? Logo { get; set; } // برای فایل لوگو
        public string? BannerRout { get; set; }
        /// <summary>
        /// آپلود فایل بنر
        /// </summary>
        public IFormFile? Banner { get; set; }
        public string? Tel { get; set; }
        public bool IsLogoChanged { get; set; }
        public bool IsBannerChanged { get; set; }
        public bool SendRequest { get; set; }
        public bool IsCompany { get; set; }

        public CompanyEditProfileDto MapToDto(CompanyEditProfileVM company)
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
                LogoRout = company.LogoRout,
                BannerRout = company.BannerRout,
                Tel = company.Tel
            };
        }

        public CompanyEditProfileVM MapToViewModel(CompanyEditProfileDto company)
        {
            return new CompanyEditProfileVM
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
                BannerRout = company.BannerRout
            };
        }

    }
}
