using AQS_Application.Dtos.IdentityServiceDto;
using System.Security.Claims;

namespace AQS_Application.Interfaces.IServices.IdentityServices
{
    public interface ILoginService
    {
        Task<LoginResultDto> LoginWithPasswordAsync(string mobile, string password);
        Task<LoginResultDto> LoginWithOtp(string mobile);
        Task LogoutAsync();
        Task<long> GetUserIdAsync(ClaimsPrincipal user);
        Task<bool> IsValidPassword(long userId, string currentPass);
        Task<string?> GetUserByClaims(ClaimsPrincipal user);
    }
}
