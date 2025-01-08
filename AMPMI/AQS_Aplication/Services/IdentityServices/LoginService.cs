using AQS_Application.Dtos.IdentityServiceDto;
using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Application.Interfaces.IServices.IdentityServices;
using AQS_Domin.Entities.Accounting;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AQS_Application.Services.IdentityServices
{
    public class LoginService : ILoginService
    {

        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ICompanyService _companyService;

        public LoginService(
         SignInManager<User> signInManager,
         UserManager<User> userManager,
         ICompanyService companyService
         )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _companyService = companyService;
        }
        public async Task<long> GetUserIdAsync(ClaimsPrincipal user)
        {
            var appUser = await _userManager.GetUserAsync(user);
            return Convert.ToInt64(appUser?.Id);
        }
        public async Task<string?> GetManagerNameByClaims(ClaimsPrincipal user)
        {
            var appUser = await _userManager.GetUserAsync(user);

            if (appUser == null)
            {
                return "";
            }

            var userId = Convert.ToInt64(appUser.Id);
            var company = await _companyService.ReadByIdAsync(userId);

            return company?.ManagerName;
        }
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

            if (await _userManager.IsLockedOutAsync(user))
            {
                return new LoginResultDto
                {
                    IsSuccess = false,
                    Message = LoginOutPutMessegeEnum.LockedOut
                };
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
            {
                return new LoginResultDto
                {
                    IsSuccess = false,
                    Message = LoginOutPutMessegeEnum.InvalidPassword
                };
            }

            // خروج از سیستم برای جلوگیری از نشست‌های فعال قبلی
            await _signInManager.SignOutAsync();

            // انجام لاگین
            await _signInManager.SignInAsync(user, isPersistent: true);

            var roles = await _userManager.GetRolesAsync(user);
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
            // بررسی صحت ورودی
            if (string.IsNullOrEmpty(mobile))
            {
                return new LoginResultDto
                {
                    IsSuccess = false,
                    Message = LoginOutPutMessegeEnum.Invalid
                };
            }

            // پیدا کردن شرکت مربوط به شماره موبایل
            var company = await _companyService.ReadByMobileNumber(mobile);
            if (company == null || company.Id == 0)
            {
                return new LoginResultDto
                {
                    IsSuccess = false,
                    Message = LoginOutPutMessegeEnum.UserNotFound
                };
            }

            // پیدا کردن کاربر مرتبط
            var user = await _userManager.FindByIdAsync(company.Id.ToString());
            if (user == null)
            {
                return new LoginResultDto
                {
                    IsSuccess = false,
                    Message = LoginOutPutMessegeEnum.UserNotFound
                };
            }

            // بررسی وضعیت قفل کاربر
            if (await _userManager.IsLockedOutAsync(user))
            {
                return new LoginResultDto
                {
                    IsSuccess = false,
                    Message = LoginOutPutMessegeEnum.LockedOut
                };
            }

            // خروج از سشن قبلی (در صورت وجود)
            await _signInManager.SignOutAsync();

            // انجام لاگین جدید
            await _signInManager.SignInAsync(user, isPersistent: true);

            // بازیابی نقش‌های کاربر
            var roles = await _userManager.GetRolesAsync(user);
            return new LoginResultDto
            {
                IsSuccess = true,
                UserId = user.Id,
                Role = roles.FirstOrDefault() ?? "None",
                Message = LoginOutPutMessegeEnum.LoginSuccessful
            };
        }

        public async Task<bool> IsValidPassword(long userId, string currentPass)
        {
            User user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) { return false; }
            return await _userManager.CheckPasswordAsync(user, currentPass);
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
