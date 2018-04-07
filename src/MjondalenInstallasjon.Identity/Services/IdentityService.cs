using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MjondalenInstallasjon.Identity.Models;

namespace MjondalenInstallasjon.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IdentityService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateUser(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            return await _userManager.CreateAsync(user, model.Password);
        }

        public async Task<IdentityResult> DeleteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return await _userManager.DeleteAsync(user);
        }
        
        public async Task<SignInResult> SignInUser(SignInViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, lockoutOnFailure: false);
        }

        public async Task SignOutUser()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ChangeUserPassword(ChangePasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            return await _userManager.ResetPasswordAsync(user, code, model.Password);
        }
    }
}