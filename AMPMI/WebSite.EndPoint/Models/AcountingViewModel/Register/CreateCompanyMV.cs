using AQS_Aplication.Dtos;
using System.ComponentModel.DataAnnotations;

namespace WebSite.EndPoint.Models.AcountingViewModel.Register
{
    public class CreateCompanyMV
    {
        /// <summary>
        /// شماره موبایل
        /// </summary>
        [Required(ErrorMessage = "شماره موبایل وارد نشده است.")]
        [RegularExpression(@"^(\+98|0)?9\d{9}$", ErrorMessage = "شماره موبایل نامعتبر است.")]
        public required string Mobile { get; set; }

        /// <summary>
        /// نام شرکت
        /// </summary>
        [Required(ErrorMessage = "نام شرکت وارد نشده است.")]
        [StringLength(100, ErrorMessage = "نام شرکت نمی‌تواند بیشتر از 100 کاراکتر باشد.")]
        public required string CompanyName { get; set; }

        /// <summary>
        /// نام مدیر
        /// </summary>
        [Required(ErrorMessage = "نام مدیر وارد نشده است.")]
        [StringLength(100, ErrorMessage = "نام مدیر نمی‌تواند بیشتر از 100 کاراکتر باشد.")]
        public required string ManagerName { get; set; }

        /// <summary>
        /// ایمیل
        /// </summary>
        [Required(ErrorMessage = "ایمیل وارد نشده است.")]
        [EmailAddress(ErrorMessage = "ایمیل نامعتبر است.")]
        public required string Email { get; set; }

        /// <summary>
        /// آدرس
        /// </summary>
        [Required(ErrorMessage = "آدرس وارد نشده است.")]
        public required string Address { get; set; }

        /// <summary>
        /// رمز عبور
        /// </summary>
        [Required(ErrorMessage = "رمز عبور وارد نشده است.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "رمز عبور باید حداقل 6 و حداکثر 50 کاراکتر باشد.")]
        public required string Password { get; set; }

        /// <summary>
        /// تأیید رمز عبور
        /// </summary>
        [Required(ErrorMessage = "تکرار رمز عبور وارد نشده است.")]
        [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن باید یکسان باشد.")]
        public required string PasswordConfirm { get; set; }

        public RegisterIdentityDTO ToRegisterIdentityDTO()
        {
            return new RegisterIdentityDTO
            {
                Mobile = this.Mobile,
                CompanyName = this.CompanyName,
                ManagerName = this.ManagerName,
                Email = this.Email,
                Password = this.Password,
                Address = this.Address
            };
        }
    }

}