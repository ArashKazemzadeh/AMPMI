using AQS_Aplication.Dtos;
using Microsoft.AspNetCore.Identity;

namespace AQS_Aplication.Interfaces.IServisces.IdentityServices
{
    public interface ILoginService
    {
        /// <summary>
        /// لاگین با حالت انتخاب تیک مرا به یاد آور
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="rememberMe"></param>
        /// <returns></returns>
        Task<SignInResult> LoginAsync(string username, string password, bool rememberMe);
        /// <summary>
        /// لاگین در این حالت باید همیشه با وارد کردن گذرواژه همراه باشد 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<LoginResultDto> LoginWithoutRememberAsync(string username, string password);
        /// <summary>
        /// لاگین با پیامک 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<LoginResultDto> LoginWithOtp(string mobile);
        /// <summary>
        /// خروج کاربر
        /// </summary>
        /// <returns></returns>
        Task LogoutAsync();
    }
}
