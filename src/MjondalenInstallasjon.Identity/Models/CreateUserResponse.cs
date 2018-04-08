using Microsoft.AspNetCore.Identity;

namespace MjondalenInstallasjon.Identity.Models
{
    public class CreateUserResponse
    {
        public IdentityResult Result { get; set; }
        public ApplicationUser CreatedUser { get; set; }
    }
}