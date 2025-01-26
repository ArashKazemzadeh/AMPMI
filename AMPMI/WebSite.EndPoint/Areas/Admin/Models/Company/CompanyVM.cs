using AQS_Application.Dtos.BaseServiceDto.Company;
using Domin.Entities;
using System.ComponentModel.DataAnnotations;

namespace WebSite.EndPoint.Areas.Admin.Models.Company
{
    public class CompanyVM
    {
        //public string   Password { get; set; } = null!;
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
        public string? LogoRout { get; set; }
        public IFormFile? Logo { get; set; }
        public string? Tel { get; set; }
        public bool IsLogoChanged { get; set; }
        public bool IsCompany { get; set; }
        public string? Website { get; set; }

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
                Website = company.Website
            };
        }
    }
}
