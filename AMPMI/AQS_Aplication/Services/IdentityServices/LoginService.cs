using AQS_Aplication.Dtos;
using AQS_Aplication.Dtos.IdentityServiceDto;
using AQS_Aplication.Interfaces.IServisces;
using AQS_Aplication.Interfaces.IServisces.BaseServices;
using AQS_Aplication.Interfaces.IServisces.IdentityServices;
using AQS_Aplication.Services;
using AQS_Common.Enums;
using AQS_Domin.Entities.Acounting;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace YourNamespace.Services
{
    public class LoginService
        (
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        ICompanyService companyService
        ) : ILoginService
    {
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly UserManager<User> _userManager = userManager;
        private readonly RoleManager<Role> _roleManager = roleManager;
        private readonly ICompanyService _companyService = companyService;
        public async Task<SignInResult> LoginAsync(string username, string password, bool rememberMe)
        {
            return await _signInManager.PasswordSignInAsync(username, password, rememberMe, false);
        }
        public async Task<LoginResultDto> LoginWithoutRememberAsync(string mobile, string password)
        {
            if (string.IsNullOrEmpty(mobile) || string.IsNullOrEmpty(password))
            {
                return new LoginResultDto
                {
                    IsSuccess = false,
                    Message = LoginOutPutMessegeEnum.Invalid
                };
            }
            var user = await _userManager.FindByNameAsync(username);
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return new LoginResultDto
                {
                    IsSuccess = false,
                    Message = LoginOutPutMessegeEnum.UserNotFound
                };
            }

            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return new LoginResultDto
                {
                    IsSuccess = true,
                    UserId = user.Id,
                    Role = roles.FirstOrDefault() ?? "None",
                    Message = LoginOutPutMessegeEnum.LoginSuccessful
                };
            }
            else
            {
                return new LoginResultDto
                {
                    IsSuccess = false,
                    Message = result.IsLockedOut ?
                    LoginOutPutMessegeEnum.LockedOut : LoginOutPutMessegeEnum.Invalid
                };
            }
        }
        public async Task<LoginResultDto> LoginWithOtp(string mobile)
        {

            var company = await _companyService.ReadByMobileNumber(mobile);
            if (company == null)
            {
                return new LoginResultDto
                {
                    IsSuccess = false,
                    Message = LoginOutPutMessegeEnum.UserNotFound
                };
            }
            var user = await _userManager.FindByIdAsync(company.Id.ToString());
            if (user == null)
            {
                return new LoginResultDto
                {
                    IsSuccess = false,
                    Message = LoginOutPutMessegeEnum.UserNotFound
                };
            }
            else
            {
                await _signInManager.SignInAsync(user, true);
                var roles = await _userManager.GetRolesAsync(user);
                return new LoginResultDto
                {
                    IsSuccess = true,
                    UserId = user.Id,
                    Role = roles.FirstOrDefault() ?? "None",
                    Message = LoginOutPutMessegeEnum.LoginSuccessful
                };
            }
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }

}
