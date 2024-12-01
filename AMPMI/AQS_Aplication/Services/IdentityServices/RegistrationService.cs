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

        public async Task<IdentityResult> RegisterAsync(string username, string email, string password)
        {
            var user = new User
            {
                UserName = username,
                Email = email
            };

            return await _userManager.CreateAsync(user, password);
        }

        public async Task AssignRoleAsync(User user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
        }
    }
}
