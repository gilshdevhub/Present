using API.Helpers.Validators;

namespace API.Dtos.Compensations;

public class CompensationSearchRequestDto
{
    [DateTimeValidator(FieldName = "FromDate")]
    public DateTime FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public class CompensationSearchResponsetDto
{
    public IEnumerable<CompensationSearchResult> CompensationsCard { get; set; }
}

public class CompensationSearchResult
{
    public long SmartCard { get; set; }
    public string PhoneNumber { get; set; }
    public int OriginCode { get; set; }
    public int DestinationCode { get; set; }
    public DateTime CardValidityFromDate { get; set; }
    public DateTime CardValidityToDate { get; set; }
    public DateTime CardRecievedDate { get; set; }
    public string QRCode { get; set; }
    public long RequestId { get; set; }
    public string CardNumber { get; set; }
}
