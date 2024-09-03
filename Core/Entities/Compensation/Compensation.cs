using Core.Entities.Vouchers;

namespace Core.Entities.Compensation;

public class Compensation
{
    public int Id { get; set; }
    public long SmartCard { get; set; }
    public long RequestId { get; set; }
    public string? PhoneNumber { get; set; }
    public int OriginStationId { get; set; }
    public int DestinationStationId { get; set; }
    public string? CardNumber { get; set; }
    public DateTime CardValidityFromDate { get; set; }
    public DateTime CardValidityToDate { get; set; }
    public DateTime CardrecievedDate { get; set; }
    public string? QRCode { get; set; }
    public int Status { get; set; } = 1;

    public required Station OriginStation { get; set; }
    public required Station DestinationStation { get; set; }
}

public class SearchCompensation
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? PhoneNumber { get; set; }
}
