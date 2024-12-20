using AQS_Aplication.Dtos;
using AQS_Aplication.Interfaces.IInfrastructure.IContext;
using AQS_Aplication.Interfaces.IServisces;
using AQS_Aplication.Interfaces.IServisces.IdentityServices;
using AQS_Domin.Entities.Acounting;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace YourNamespace.Services
{
    public class RegistrationService(UserManager<User> userManager, RoleManager<Role> roleManager, ICompanyService companyService, IDbAmpmiContext context) : IRegistrationService
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly RoleManager<Role> _roleManager = roleManager;
        private readonly ICompanyService _companyService = companyService;

        public async Task<ResultRegisterIdentityDto> RegisterAsync(RegisterIdentityDTO registerIdentityDTO, string role)
        {
            User? user = null;
            try
            {
                string errorMessages = string.Empty;

                // بررسی وجود کاربر با ایمیل
                var existingUserByEmail = await _userManager.FindByEmailAsync(registerIdentityDTO.Email);
                if (existingUserByEmail != null)
                {
                    return new ResultRegisterIdentityDto
                    {
                        userId = 0,
                        errorMessage = "این ایمیل قبلاً ثبت شده است."
                    };
                }

                // بررسی وجود کاربر با نام کاربری
                var existingUserByPhone = await _userManager.FindByNameAsync(registerIdentityDTO.CompanyName.Trim());
                if (existingUserByPhone != null)
                {
                    return new ResultRegisterIdentityDto
                    {
                        userId = 0,
                        errorMessage = "نام کاربری موجود است"
                    };
                }

                // ایجاد کاربر جدید
                user = new User
                {
                    UserName = registerIdentityDTO.CompanyName.Trim(),
                    Email = registerIdentityDTO.Email.Trim(),
                    PhoneNumber = registerIdentityDTO.Mobile.Trim(),
                };

                // بررسی وجود نقش
                var existingRole = await _roleManager.FindByNameAsync(role);
                if (existingRole == null)
                {
                    var roleResult = await _roleManager.CreateAsync(new Role { Name = role });
                    if (!roleResult.Succeeded)
                    {
                        var roleErrors = roleResult.Errors.Select(e => e.Description).ToList();
                        errorMessages = $"{string.Join(", ", roleErrors)}";
                        return new ResultRegisterIdentityDto
                        {
                            userId = 0,
                            errorMessage = errorMessages
                        };
                    }
                }

                // ایجاد کاربر
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

                // افزودن کاربر به نقش
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

                // ایجاد شرکت در سیستم
                long id = await _companyService.Create(registerIdentityDTO, user.Id);

                if (id != user.Id)
                {
                    throw new Exception("ثبت نام انجام نشد.");
                }



                return new ResultRegisterIdentityDto
                {
                    userId = id,
                    errorMessage = string.Empty
                };
            }
            catch (Exception ex)
            {

                if (user != null && user.Id > 0)
                {
                    await _userManager.DeleteAsync(user);
                    if (await _companyService.IsExistById(user.Id))
                        await _userManager.DeleteAsync(user);
                }

                return new ResultRegisterIdentityDto
                {
                    userId = 0,
                    errorMessage = "خطا در انجام عملیات: " + ex.Message
                };
            }
        }

    }

}

