using System.ComponentModel.DataAnnotations;

namespace WebSite.EndPoint.Areas.Company.Models.Profile
{
    public class PasswordEditVm
    {
        /// <summary>
        /// رمز عبور فعلی
        /// </summary>
        [Required(ErrorMessage = "گذرواژه فعلی وارد نشده است.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "گذرواژه  باید حداقل 6 و حداکثر 50 کاراکتر باشد.")]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// رمز عبور
        /// </summary>
        [Required(ErrorMessage = "گذرواژه وارد نشده است.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "گذرواژه  باید حداقل 6 و حداکثر 50 کاراکتر باشد.")]
        public string NewPassword { get; set; }

        /// <summary>
        /// تأیید رمز عبور
        /// </summary>
        [Required(ErrorMessage = "تکرار گذرواژه وارد نشده است.")]
        [Compare("NewPassword", ErrorMessage = "گذرواژه  و تکرار آن باید یکسان باشد.")]
        public string ComparePassword { get; set; }
        
    }
}
