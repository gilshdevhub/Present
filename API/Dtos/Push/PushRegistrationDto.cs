using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Push;

public class PushRegistrationRequestDto
{
    [MaxLength(200)]
    public string TokenId { get; set; }
    [MaxLength(100)]
    public string HWId { get; set; }
}
public class PushRegistrationResponseDto
{
    public int PushRegistrationId { get; set; }
}