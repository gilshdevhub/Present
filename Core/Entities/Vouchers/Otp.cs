namespace Core.Entities.Vouchers;

public class OtpRequest
{
    public string UserId { get; set; }
    public string PhoneNumber { get; set; }
}

public class OtpResponse
{
    public bool IsSuccess { get; set; }
    public string Description { get; set; }
}
