using System.ComponentModel.DataAnnotations;

namespace TheProjectTascamon.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }


        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The username must be between 3 and 20 characters long.")]
        [Display(Name = "Username")]
        public string? Username { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
