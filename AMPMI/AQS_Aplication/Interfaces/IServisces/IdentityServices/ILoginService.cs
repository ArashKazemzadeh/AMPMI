using AQS_Application.Dtos.IdentityServiceDto;
using System.Security.Claims;

namespace AQS_Application.Interfaces.IServices.IdentityServices
{
    public interface ILoginService
    {
        Task<LoginResultDto> LoginWithPasswordAsync(string mobile, string password);
        Task<LoginResultDto> LoginWithOtp(string mobile);
        Task LogoutAsync();
        /// <summary>
        /// شناسه کاربر لاگین شده را بر میگرداند
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<long> GetUserIdAsync(ClaimsPrincipal user);
        Task<bool> IsValidPassword(long userId, string currentPass);
        /// <summary>
        /// نام مدیر را بر اساس کلیم های کاربر برمیگرداند
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string?> GetManagerNameByClaims(ClaimsPrincipal user);
    }
}
