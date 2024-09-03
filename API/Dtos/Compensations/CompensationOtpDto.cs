using API.Helpers.Validators;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Compensations;

public class CompensationOtpRequestDto
{
    [MinLength(10)]
    [LengthValidator(FieldName="PhoneNumber")]
    [OtpPhoneNumberValidator]
    public string PhoneNumber { get; set; }
}

public class CompensationOtpSearchRequestDto : CompensationOtpRequestDto
{
    public string Otp { get; set; }
}
