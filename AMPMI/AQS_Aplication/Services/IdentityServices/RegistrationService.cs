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

            var user = new User
            {
                NormalizedUserName = registerIdentityDTO.ManagerName.Trim().ToUpper(),
                UserName = registerIdentityDTO.CompanyName.Trim(),
                Email = registerIdentityDTO.Email.Trim(),
                PhoneNumber = registerIdentityDTO.Mobile.Trim(),
            };

            string errorMessages = string.Empty;

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

            return new ResultRegisterIdentityDto
            {
                userId = user.Id,
                errorMessage = string.Empty
            };
        }


    }
}
