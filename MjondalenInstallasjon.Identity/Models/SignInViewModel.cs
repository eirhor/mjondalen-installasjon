﻿using System.ComponentModel.DataAnnotations;

namespace MjondalenInstallasjon.Identity.Models
{
    public class SignInViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}