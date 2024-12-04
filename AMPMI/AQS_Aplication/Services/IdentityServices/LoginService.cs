using AQS_Domin.Entities.Acounting;
using Microsoft.AspNetCore.Identity;

namespace YourNamespace.Services
{
    public class LoginService
    {
        private readonly SignInManager<User> _signInManager;

        public LoginService(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<SignInResult> LoginAsync(string username, string password, bool rememberMe)
        {
            return await _signInManager.PasswordSignInAsync(username, password, rememberMe, false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
