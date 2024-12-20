using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace WebSite.EndPoint.Models.AcountingViewModel.Login
{
    public class LoginVM
    {
        /// <summary>
        /// شماره موبایل
        /// </summary>
        [Required(ErrorMessage = "شماره موبایل وارد نشده است.")]
        [RegularExpression(@"^(\+98|0)?9\d{9}$", ErrorMessage = "شماره موبایل نامعتبر است.")]
        public required string Mobile { get; set; }
        [Required(ErrorMessage = "رمز عبور وارد نشده است.")]
        /// <summary>
        /// گذرواژه
        /// </summary>
        public required string Password { get; set; }
    }
}
