using AQS_Domin.Entities.Acounting;
using Microsoft.AspNetCore.Identity;

namespace YourNamespace.Services
{
    public class RegistrationService
    {
        private readonly UserManager<User> _userManager;

        public RegistrationService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> RegisterAsync
            (string mobile,string companyname,string managerName, string email, string password , string address)
        {
            var user = new User
            {
                NormalizedUserName = managerName,
                UserName = companyname,
                Email = email,
                PhoneNumber = mobile,
                PasswordHash = password,
            };
            return await _userManager.CreateAsync(user, password);
        }
        public async Task<IdentityResult> AssignRoleAsync(User user, string role)
        {
           return await _userManager.AddToRoleAsync(user, role);
        }
    }
}
