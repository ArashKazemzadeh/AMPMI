using System.ComponentModel.DataAnnotations;

namespace WebSite.EndPoint.Models.AcountingViewModel.Register
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "شماره تلفن را وارد نمایید")]
        [Display(Name = "شماره تلفن")]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "نام شرکت را وارد نمایید")]
        [Display(Name = "نام شرکت")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "نام مدیر را وارد نمایید")]
        [Display(Name = "نام مدیر")]
        public string ManagerName { get; set; }
        [Required(ErrorMessage = "ایمیل را وارد نمایید")]
        [Display(Name = "ایمیل")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "آدرس را وارد نمایید")]
        [Display(Name = "آدرس")]
        public string Address { get; set; }
        [Required(ErrorMessage = "پسورد را وارد نمایید")]
        [Display(Name = "پسورد")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "تکرار پسورد را وارد نمایید")]
        [Display(Name = "تکرار پسورد")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "تکرار پسورد باید با پسورد یکی باشد")]
        public string PasswordConfirm { get; set; }
    }
}
