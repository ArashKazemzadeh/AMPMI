namespace WebSite.EndPoint.Models.AccountingViewModel.Register;



public class ConfirmOtpViewModel
{
    public string Mobile { get; set; }
    public int UserOtp { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;

}

