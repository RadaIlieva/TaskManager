namespace AutenticationAutorizationAPI.Models
{
    public class RegisterUserDto
    {
        public string Name { get; set; }
        public string EmailOrPhone { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
