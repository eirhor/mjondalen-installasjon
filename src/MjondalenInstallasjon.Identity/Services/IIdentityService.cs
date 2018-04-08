using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MjondalenInstallasjon.Identity.Models;

namespace MjondalenInstallasjon.Identity.Services
{
    public interface IIdentityService
    {
        Task<CreateUserResponse> CreateUser(RegisterViewModel model);
        Task<IdentityResult> DeleteUser(string email);
        Task<SignInResult> SignInUser(SignInViewModel model);
        Task SignOutUser();
        Task<IdentityResult> ChangeUserPassword(ChangePasswordViewModel model);
        Task<IdentityResult> CreateRole(string roleName);
        Task<bool> RoleExists(string roleName);
        Task<IdentityResult> AddUserToRole(ApplicationUser user, string roleName);
        Task<IList<ApplicationUser>> GetUsersInRole(string roleName);
    }
}