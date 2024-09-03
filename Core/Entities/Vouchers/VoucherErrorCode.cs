namespace Core.Entities.Vouchers;

public class VoucherErrorCode
{
    public int Id { get; set; }
    public string LegacyErrorCode { get; set; }
    public int ErrorCode { get; set; }
    public string ErrorDescription { get; set; }
}
