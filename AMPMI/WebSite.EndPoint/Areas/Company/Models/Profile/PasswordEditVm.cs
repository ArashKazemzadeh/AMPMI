using System.ComponentModel.DataAnnotations;

namespace WebSite.EndPoint.Areas.Company.Models.Profile
{
    public class PasswordEditVm
    {
        /// <summary>
        /// رمز عبور فعلی
        /// </summary>
        [Required(ErrorMessage = "رمز عبور وارد نشده است.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "رمز عبور باید حداقل 6 و حداکثر 50 کاراکتر باشد.")]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// رمز عبور
        /// </summary>
        [Required(ErrorMessage = "رمز عبور وارد نشده است.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "رمز عبور باید حداقل 6 و حداکثر 50 کاراکتر باشد.")]
        public string NewPassword { get; set; }

        /// <summary>
        /// تأیید رمز عبور
        /// </summary>
        [Required(ErrorMessage = "تکرار رمز عبور وارد نشده است.")]
        [Compare("NewPassword", ErrorMessage = "رمز عبور و تکرار آن باید یکسان باشد.")]
        public string ComparePassword { get; set; }

    }
}
