using System.ComponentModel.DataAnnotations;

namespace API.Helpers.Validators;

[AttributeUsage( AttributeTargets.Property)]
public class DateTimeValidatorAttribute : ValidationAttribute
{
    public string FieldName { get; set; }
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is not DateTime)
        {
            return new ValidationResult($"property [{ FieldName }] has an invalid date time format");
        }
        else if ((DateTime)value == DateTime.MinValue)
        {
            return new ValidationResult($"property [{ FieldName }] has an invalid date time value");
        }

        return ValidationResult.Success;
    }
}
