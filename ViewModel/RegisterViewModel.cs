﻿using System.ComponentModel.DataAnnotations;

namespace TheProjectTascamon.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The username must be between 3 and 20 characters long.")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
