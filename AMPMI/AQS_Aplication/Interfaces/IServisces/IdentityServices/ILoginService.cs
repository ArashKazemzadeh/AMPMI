using AQS_Application.Dtos.IdentityServiceDto;

namespace AQS_Application.Interfaces.IServices.IdentityServices
{
    public interface ILoginService
    {
        Task<LoginResultDto> LoginWithPasswordAsync(string mobile, string password);
        Task<LoginResultDto> LoginWithOtp(string mobile);
        Task LogoutAsync();
    }
}
