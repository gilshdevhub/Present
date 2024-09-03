using System.ComponentModel.DataAnnotations;

namespace Core.Helpers.Validators;

[AttributeUsage(AttributeTargets.Property)]
public class EnumRangeValidatorAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("field is required");
        }

        var type = value.GetType();

        if (!Enum.IsDefined(type, value))
        {
            int[] enumValues = (int[])Enum.GetValues(type);

            return new ValidationResult($"the field {type.Name} must be between {enumValues[0]} and {enumValues[^1]}");
        }

        return ValidationResult.Success;
    }
}
