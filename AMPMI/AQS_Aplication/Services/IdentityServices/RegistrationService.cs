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
        private readonly IDbAmpmiContext _context = context;

        public async Task<ResultRegisterIdentityDto> RegisterAsync(RegisterIdentityDTO registerIdentityDTO, string role)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
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

                    // بررسی وجود کاربر با شماره موبایل
                    var existingUserByPhone = await _userManager.FindByNameAsync(registerIdentityDTO.Mobile.Trim());
                    if (existingUserByPhone != null)
                    {
                        return new ResultRegisterIdentityDto
                        {
                            userId = 0,
                            errorMessage = "این شماره موبایل قبلاً ثبت شده است."
                        };
                    }

                    // ایجاد کاربر جدید
                    var user = new User
                    {
                        UserName = registerIdentityDTO.CompanyName.Trim(),
                        Email = registerIdentityDTO.Email.Trim(),
                        PhoneNumber = registerIdentityDTO.Mobile.Trim(),
                    };

                    // بررسی وجود نقش
                    var existingRole = await _roleManager.FindByNameAsync(role);
                    if (existingRole == null)
                    {
                        // اگر نقش وجود ندارد، ایجاد آن
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
                    //ایجاد کاربر به عنوان قطعه ساز
                    long id = await _companyService.Create(registerIdentityDTO, user.Id);

                    if (id != user.Id)
                    {
                        await transaction.RollbackAsync();
                        return new ResultRegisterIdentityDto
                        {
                            userId = 0,
                            errorMessage = "ثبت نام انجام نشد"
                        };
                    }

                    // در صورت موفقیت آمیز بودن، تراکنش را commit می‌کنیم
                    await transaction.CommitAsync();

                    // بازگشت نتیجه
                    return new ResultRegisterIdentityDto
                    {
                        userId = id,
                        errorMessage = string.Empty
                    };
                }
                catch (Exception ex)
                {
                    // در صورت بروز خطا، تراکنش به صورت خودکار rollback می‌شود
                    await transaction.RollbackAsync();
                    return new ResultRegisterIdentityDto
                    {
                        userId = 0,
                        errorMessage = "خطا در انجام عملیات: " + ex.Message
                    };
                }
            }
        }

    }
}
