
using System.ComponentModel.DataAnnotations;

namespace AutenticationAutorizationAPI.Models
{
    public class RegisterUserDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string UserEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
