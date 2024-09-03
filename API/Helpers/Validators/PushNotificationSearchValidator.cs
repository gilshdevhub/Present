using API.Dtos.Push;
using System.ComponentModel.DataAnnotations;

namespace API.Helpers.Validators;

[AttributeUsage(AttributeTargets.Class)]
public class PushNotificationSearchValidatorAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        PushNotificationRequestQueryDto pushNotificationRequestQueryDto = validationContext.ObjectInstance as PushNotificationRequestQueryDto;

        if (pushNotificationRequestQueryDto.PushRegistrationId < 1)
        {
            return new ValidationResult("ערך לא תקין עבור מספר רישום");
        }
                
        return ValidationResult.Success;
    }
}
