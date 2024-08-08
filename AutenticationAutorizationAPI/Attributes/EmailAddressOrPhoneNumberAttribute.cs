using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AutenticationAutorizationAPI.Attributes
{
    public class EmailAddressOrPhoneNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var input = value as string;

            if (string.IsNullOrEmpty(input))
            {
                return new ValidationResult("Email or Phone Number is required.");
            }

            var isValidEmail = new EmailAddressAttribute().IsValid(input);
            var isValidPhone = Regex.IsMatch(input, @"^\d{10}$"); 

            if (isValidEmail || isValidPhone)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid Email or Phone Number.");
        }
    }
}
