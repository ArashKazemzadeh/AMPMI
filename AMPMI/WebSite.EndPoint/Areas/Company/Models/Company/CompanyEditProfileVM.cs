using AQS_Application.Dtos.BaseServiceDto.Company;
using System.ComponentModel.DataAnnotations;

namespace WebSite.EndPoint.Areas.Company.Models.Company
{
    public class CompanyEditProfileVM
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "نام شرکت نمیتواند خالی باشد")]
        public string Name { get; set; }
        [Required(ErrorMessage = "نام مدیر نمیتواند خالی باشد")]
        public string ManagerName { get; set; }
        [Required(ErrorMessage = "شماره تلفن نمیتواند خالی باشد")]
        public string MobileNumber { get; set; }
        [Required(ErrorMessage = "ایمیل نمیتواند خالی باشد")]
        public string Email { get; set; }
        [Required(ErrorMessage = "آدرس نمیتواند خالی باشد")]
        public string Address { get; set; }
        public string? Brands { get; set; }
        public int Capacity { get; set; }
        public string? Partnership { get; set; }
        public string? QualityGrade { get; set; }
        public string? Iso { get; set; }
        public string? About { get; set; }
        public string? Website { get; set; }

        public string? LogoRout { get; set; } = string.Empty;
        public IFormFile? Logo { get; set; } // برای فایل لوگو
        public string? Tel { get; set; }
        public bool IsLogoChanged { get; set; }
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
                Tel = company.Tel,
                Website = company.Website
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
                Website = company.Website
            };
        }

    }
}
