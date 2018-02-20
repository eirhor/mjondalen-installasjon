using System.ComponentModel.DataAnnotations;

namespace MjondalenInstallasjon.Identity.Models
{
    public class ChangePasswordViewModel
    {
        public string Email { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "Passordet må ha minst {2} og maks {1} i lengde.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Passord")]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passordene må matche.")]
        [Display(Name = "Bekreft passord")]
        public string ConfirmPassword { get; set; }
    }
}