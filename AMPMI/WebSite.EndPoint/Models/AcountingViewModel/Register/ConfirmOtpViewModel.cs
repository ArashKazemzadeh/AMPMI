using System.ComponentModel.DataAnnotations;

namespace WebSite.EndPoint.Models.AcountingViewModel.Register
{
    public class ConfirmOtpViewModel
    {
        public required string Mobile { get; set; }
        public int UserOtp { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

    }
}
