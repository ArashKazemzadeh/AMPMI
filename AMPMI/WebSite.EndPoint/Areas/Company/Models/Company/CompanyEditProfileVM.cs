using AQS_Application.Dtos.BaseServiceDto.Company;
using System.ComponentModel.DataAnnotations;

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

        ///// <summary>
        ///// رمز عبور فعلی
        ///// </summary>
        //[Required(ErrorMessage = "گذرواژه فعلی وارد نشده است.")]
        //[StringLength(50, MinimumLength = 6, ErrorMessage = "گذرواژه  باید حداقل 6 و حداکثر 50 کاراکتر باشد.")]
        //public string CurrentPassword { get; set; }
        ///// <summary>
        ///// تأیید رمز عبور
        ///// </summary>
        //[Required(ErrorMessage = "تکرار گذرواژه وارد نشده است.")]
        //[Compare("NewPassword", ErrorMessage = "گذرواژه  و تکرار آن باید یکسان باشد.")]
        //public string ComparePassword { get; set; }
        public string? Brands { get; set; }

        public int Capacity { get; set; }

        public string? Partnership { get; set; }

        public string? QualityGrade { get; set; }

        public string? Iso { get; set; }

        public string? About { get; set; }

        public string? LogoRout { get; set; }
        public IFormFile Logo { get; set; } // برای فایل لوگو
        public bool IsPictureChanged { get; set; }
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
            };
        }

    }
}
