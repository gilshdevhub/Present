namespace Core.Entities;

public class Otp
{
    public int Id { get; set; }
    public string OtpCode { get; set; }
    public string PhoneNumber { get; set; }
    public string SystemName { get; set; }
    public DateTime CreationDate { get; set; }
}
