using AQS_Aplication.Dtos;
using AQS_Aplication.Interfaces.IServisces.IdentityServices;
using AQS_Domin.Entities.Acounting;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace YourNamespace.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly UserManager<User> _userManager;

        public RegistrationService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResultRegisterIdentityDto> RegisterAsync(RegisterIdentityDTO registerIdentityDTO, string role)
        {
            // بررسی وجود کاربر با همان ایمیل یا شماره موبایل
            var existingUserByEmail = await _userManager.FindByEmailAsync(registerIdentityDTO.Email);
            if (existingUserByEmail != null)
            {
                return new ResultRegisterIdentityDto
                {
                    userId = 0,
                    errorMessage = "این ایمیل قبلاً ثبت شده است."
                };
            }

            var existingUserByPhone = await _userManager.FindByNameAsync(registerIdentityDTO.Mobile.Trim());
            if (existingUserByPhone != null)
            {
                return new ResultRegisterIdentityDto
                {
                    userId = 0,
                    errorMessage = "این شماره موبایل قبلاً ثبت شده است."
                };
            }

            // ساخت کاربر جدید
            var user = new User
            {
                NormalizedUserName = registerIdentityDTO.ManagerName.Trim().ToUpper(),
                UserName = registerIdentityDTO.CompanyName.Trim(),
                Email = registerIdentityDTO.Email.Trim(),
                PhoneNumber = registerIdentityDTO.Mobile.Trim(),
            };

            string errorMessages = string.Empty;

            // ثبت کاربر در دیتابیس
            var resultCreate = await _userManager.CreateAsync(user, registerIdentityDTO.Password);
            if (!resultCreate.Succeeded)
            {
                var errors = resultCreate.Errors.Select(e => e.Description).ToList();
                errorMessages = $"{string.Join(", ", errors)}";
                return new ResultRegisterIdentityDto
                {
                    userId = 0,
                    errorMessage = errorMessages
                };
            }

            // اضافه کردن کاربر به نقش
            var resultAddToRole = await _userManager.AddToRoleAsync(user, role);
            if (!resultAddToRole.Succeeded)
            {
                var errors = resultAddToRole.Errors.Select(e => e.Description).ToList();
                errorMessages = $"{string.Join(", ", errors)}";
                return new ResultRegisterIdentityDto
                {
                    userId = 0,
                    errorMessage = errorMessages
                };
            }

            // در صورت موفقیت، شناسه کاربر را برمی‌گردانیم
            return new ResultRegisterIdentityDto
            {
                userId = user.Id,
                errorMessage = string.Empty
            };
        }


    }
}
