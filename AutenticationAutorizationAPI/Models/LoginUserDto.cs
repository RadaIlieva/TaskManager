using AutenticationAutorizationAPI.Attributes;

namespace AutenticationAutorizationAPI.Models
{
    public class LoginUserDto
    {
        [EmailAddressOrPhoneNumber]
        public string EmailOrPhone { get; set; }
        public string Password { get; set; }
    }
}
