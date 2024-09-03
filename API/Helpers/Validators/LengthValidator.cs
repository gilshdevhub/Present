using System.ComponentModel.DataAnnotations;

namespace API.Helpers.Validators;

public class LengthValidatorAttribute : ValidationAttribute
{
    public string FieldName { get; set; }
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var configuration = (IConfiguration)validationContext
            .GetService(typeof(IConfiguration));
        var length = int.Parse(configuration.GetSection("ValuesOptions").Value.ToString()) ;
        if (value.ToString().Length > length)
        {
            return new ValidationResult($"The fild { FieldName } must be a string or array type with a maximum length of {length}");
        }
        
        return ValidationResult.Success;
    }
}
