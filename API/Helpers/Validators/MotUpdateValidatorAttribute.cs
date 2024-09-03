using API.Dtos;
using System.ComponentModel.DataAnnotations;

namespace API.Helpers.Validators;

[AttributeUsage(AttributeTargets.Class)]
public class MotUpdateValidatorAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        MotUpdateRequestDto motUpdateRequest = validationContext.ObjectInstance as MotUpdateRequestDto;

        if (string.IsNullOrEmpty(motUpdateRequest.LineRef) && string.IsNullOrEmpty(motUpdateRequest.MonitoringRef))
        {
            return new ValidationResult("at least one field must has a value");
        }

        return ValidationResult.Success;
    }
}
