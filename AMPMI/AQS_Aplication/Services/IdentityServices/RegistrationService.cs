using AQS_Application.Dtos.IdentityServiceDto;
using AQS_Application.Interfaces.IInfrastructure.IContext;
using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Application.Interfaces.IServices.IdentityServices;
using AQS_Domin.Entities.Accounting;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Security.Claims;

namespace YourNamespace.Services
{
    public class RegistrationService(UserManager<User> userManager, RoleManager<Role> roleManager, ICompanyService companyService, SignInManager<User> signInManager, IDbAmpmiContext context) : IRegistrationService
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly RoleManager<Role> _roleManager = roleManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly ICompanyService _companyService = companyService;
        private readonly IDbAmpmiContext _context = context;
        public async Task<ResultRegisterIdentityDto> ChangePasswordAsync(long userId, string currentPassport, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new ResultRegisterIdentityDto
                {
                    userId = 0,
                    errorMessage = "کاربر موجود نیست"
                };
            }
            var result = await _userManager.ChangePasswordAsync(user, currentPassport, newPassword);
            if (result.Succeeded)
            {
                return new ResultRegisterIdentityDto
                {
                    userId = userId,
                    errorMessage = "گدرواژه باموفقیت ویرایش شد"
                };
            }
            else
            {
                return new ResultRegisterIdentityDto
                {
                    userId = userId,
                    errorMessage = "گدرواژه فعلی درست وارد نشده است"
                };
            }
        }
        public async Task<ResultRegisterIdentityDto> RegisterAsync(RegisterIdentityDTO registerIdentityDTO, string role, bool isLogin)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                User user = null;
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
                    // TODO : این شرط کار نمیکند
                    // بررسی وجود کاربر با شماره موبایل
                    var existingUserByPhone = await _userManager.FindByNameAsync(registerIdentityDTO.CompanyName.Trim());
                    if (existingUserByPhone != null)
                    {
                        return new ResultRegisterIdentityDto
                        {
                            userId = 0,
                            errorMessage = "نام شرکت قبلاً ثبت شده است."
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
                    if (isLogin)
                    {
                        await _signInManager.SignInAsync(user, true);
                    }

                    // در صورت موفقیت، تراکنش را commit می‌کنیم
                    await transaction.CommitAsync();

                    return new ResultRegisterIdentityDto
                    {
                        userId = id,
                        errorMessage = string.Empty
                    };

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    if (user != null && user.Id > 0)
                    {
                        await _userManager.DeleteAsync(user);
                        if (await _companyService.IsExistById(user.Id))
                            await _companyService.Delete(user.Id);
                    }

                    return new ResultRegisterIdentityDto
                    {
                        userId = 0,
                        errorMessage = "خطا در انجام عملیات: " + ex.Message
                    };
                }
            }
        }
        public async Task<ResultRegisterIdentityDto> RegisterAsync(RegisterIdentityDTO registerIdentityDTO, string role)
        {
            return await RegisterAsync(registerIdentityDTO, role, true);
        }
        public async Task<bool> DeleteUserAsync(long userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

}

