using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MjondalenInstallasjon.Identity.Data;
using MjondalenInstallasjon.Identity.Models;

namespace MjondalenInstallasjon.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
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

        public async Task<IdentityResult> CreateRole(string roleName)
        {
            return await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        public async Task<bool> RoleExists(string roleName)
        {
            var role = await _roleManager.GetRoleNameAsync(new IdentityRole(roleName));
            return !string.IsNullOrEmpty(role);
        }

        public async Task<IdentityResult> AddUserToRole(ApplicationUser user, string roleName)
        {
            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IList<ApplicationUser>> GetUsersInRole(string roleName)
        {
            return await _userManager.GetUsersInRoleAsync(roleName);
        }
    }
}