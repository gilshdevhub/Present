using API.Dtos.Compensations;
using System.ComponentModel.DataAnnotations;

namespace API.Helpers.Validators;

[AttributeUsage(AttributeTargets.Property)]
public class OtpPhoneNumberValidatorAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        CompensationOtpRequestDto request = validationContext.ObjectInstance as CompensationOtpRequestDto;

        if (!int.TryParse(request.PhoneNumber, out _))
        {
            return new ValidationResult("only numbers are valid");
        }

        if (!request.PhoneNumber.StartsWith("05"))
        {
            return new ValidationResult("invalid mobile phone number");
        }

        return ValidationResult.Success;
    }
}
