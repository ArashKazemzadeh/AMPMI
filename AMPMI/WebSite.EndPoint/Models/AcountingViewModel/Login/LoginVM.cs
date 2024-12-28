using System.ComponentModel.DataAnnotations;

namespace WebSite.EndPoint.Models.AccountingViewModel.Login
{
    public class LoginVM
    {/// <summary>
     /// شماره موبایل
     /// </summary>
        [Required(ErrorMessage = "شماره موبایل وارد نشده است.")]
        [RegularExpression(@"^(\+98|0)?9\d{9}$", ErrorMessage = "شماره موبایل نامعتبر است.")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "رمز عبور وارد نشده است.")]
        /// <summary>
        /// گذرواژه
        /// </summary>
        public string Password { get; set; }
    }
}
