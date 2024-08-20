using System.ComponentModel.DataAnnotations;

namespace TaskManagerProject.DTOs
{
    public class RegisterUserDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
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
