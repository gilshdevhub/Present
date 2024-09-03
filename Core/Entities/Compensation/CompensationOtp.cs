namespace Core.Entities.Compensation;

public class CompensationOtp
{
}

public class CompensationOtpRequest
{
    public string PhoneNumber { get; set; }
}

public class CompensationOtpResponse
{
    public string Token { get; set; }
}
