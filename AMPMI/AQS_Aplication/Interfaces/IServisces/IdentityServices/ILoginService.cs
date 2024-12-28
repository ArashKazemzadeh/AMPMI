using AQS_Aplication.Dtos.IdentityServiceDto;
using Microsoft.AspNetCore.Identity;

namespace AQS_Aplication.Interfaces.IServisces.IdentityServices
{
    internal interface ILoginService
    {
        Task<SignInResult> LoginAsync(string username, string password, bool rememberMe);
        Task<LoginResultDto> LoginWithoutRememberAsync(string username, string password);
        Task<LoginResultDto> LoginWithOtp(string mobile);
        Task LogoutAsync();
    }
}
