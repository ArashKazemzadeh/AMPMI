using AQS_Application.Dtos.IdentityServiceDto;
using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Application.Interfaces.IServices.IdentityServices;
using AQS_Domin.Entities.Accounting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace AQS_Application.Services.IdentityServices
{
    public class LoginService
         (
         SignInManager<User> signInManager,
         UserManager<User> userManager,
         ICompanyService companyService
         ) : ILoginService
    {

        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly UserManager<User> _userManager = userManager;
        private readonly ICompanyService _companyService = companyService;

        public async Task<LoginResultDto> LoginWithPasswordAsync(string mobile, string password)
        {
            if (string.IsNullOrEmpty(mobile) || string.IsNullOrEmpty(password))
            {
                return new LoginResultDto
                {
                    IsSuccess = false,
                    Message = LoginOutPutMessegeEnum.Invalid
                };
            }

            var company = await _companyService.ReadByMobileNumber(mobile);
            if (company == null || company.Id == 0)
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

            var pass = await _userManager.CheckPasswordAsync(user, password);
            if (!pass)
            {
                return new LoginResultDto
                {
                    IsSuccess = false,
                    Message = LoginOutPutMessegeEnum.InvalidPassword
                };
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                return new LoginResultDto
                {
                    IsSuccess = false,
                    Message = LoginOutPutMessegeEnum.LockedOut
                };
            }

            // اضافه کردن Claims شامل UserId، نقش و سایر اطلاعات
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? "None")
            };

            var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _signInManager.Context.SignInAsync(IdentityConstants.ApplicationScheme, principal);

            return new LoginResultDto
            {
                IsSuccess = true,
                UserId = user.Id,
                Role = roles.FirstOrDefault() ?? "None",
                Message = LoginOutPutMessegeEnum.LoginSuccessful
            };
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

            // اضافه کردن Claims شامل UserId، نقش و سایر اطلاعات
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? "None")
            };

            var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _signInManager.Context.SignInAsync(IdentityConstants.ApplicationScheme, principal);

            return new LoginResultDto
            {
                IsSuccess = true,
                UserId = user.Id,
                Role = roles.FirstOrDefault() ?? "None",
                Message = LoginOutPutMessegeEnum.LoginSuccessful
            };
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
