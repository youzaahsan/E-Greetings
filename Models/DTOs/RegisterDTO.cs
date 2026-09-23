using E_Greetings.Core.Validations;
using System.ComponentModel.DataAnnotations;

namespace E_Greetings.Models.DTOs
{
    public class RegisterDTO
    {
        [Required, MaxLength(80)]
        public string Name { get; set; }

        [Required, MaxLength(120)]
        [EmailAddress]
        [UniqueEmail]
        public string Email { get; set; }

        [Required, MaxLength(200)]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
