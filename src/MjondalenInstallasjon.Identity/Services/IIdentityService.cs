using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MjondalenInstallasjon.Identity.Models;

namespace MjondalenInstallasjon.Identity.Services
{
    public interface IIdentityService
    {
        Task<IdentityResult> CreateUser(RegisterViewModel model);
        Task<IdentityResult> DeleteUser(string email);
        Task<SignInResult> SignInUser(SignInViewModel model);
        Task SignOutUser();
        Task<IdentityResult> ChangeUserPassword(ChangePasswordViewModel model);
    }
}