using System.ComponentModel.DataAnnotations;

namespace WebSite.EndPoint.Areas.Company.Models.Company
{
    public class CompanyEditProfileVM
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public string ManagerName { get; set; } = null!;

        public string MobileNumber { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Address { get; set; } = null!;

        /// <summary>
        /// رمز عبور فعلی
        /// </summary>
        [Required(ErrorMessage = "گذرواژه فعلی وارد نشده است.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "گذرواژه  باید حداقل 6 و حداکثر 50 کاراکتر باشد.")]
        public string CurrentPassword { get; set; }
        /// <summary>
        /// تأیید رمز عبور
        /// </summary>
        [Required(ErrorMessage = "تکرار گذرواژه وارد نشده است.")]
        [Compare("NewPassword", ErrorMessage = "گذرواژه  و تکرار آن باید یکسان باشد.")]
        public string ComparePassword { get; set; }
        public string? Brands { get; set; }

        public int Capacity { get; set; }

        public string? Partnership { get; set; }

        public string? QualityGrade { get; set; }

        public string? Iso { get; set; }

        public string? About { get; set; }

        //public string? LogoRout { get; set; }

        //public bool IsCompany { get; set; }
   
    }
}
