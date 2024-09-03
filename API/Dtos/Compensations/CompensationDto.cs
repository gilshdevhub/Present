using API.Helpers.Validators;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Compensations;

[TicketStationValidator]
public class CompensationRequestDto
{
    [Required]
    [Range(minimum: 1, maximum: double.MaxValue)]
    public long RequestId { get; set; }
    [Required]
    [MaxLength(600)]
    public string QRCode { get; set; }
    [Required]
    [Range(minimum: 1, maximum: double.MaxValue)]
    public long SmartCard { get; set; }
    [Required]
    [MaxLength(15)]
    public string PhoneNumber { get; set; }
    [Required]
    [Range(minimum: 1, maximum: 9999)]
    public int OriginCode { get; set; }
    [Required]
    [Range(minimum: 1, maximum: 9999)]
    public int DestinationCode { get; set; }
    [Required]
    [MaxLength(20)]
    public string CardNumber { get; set; }
    [Required]
    [DateTimeValidator(FieldName = "CardValidityFromDate")]
    public DateTime CardValidityFromDate { get; set; }
    [Required]
    [DateTimeValidator(FieldName = "CardValidityToDate")]
    public DateTime CardValidityToDate { get; set; }
}

public class CompensationResponseDto
{
    public int RowId { get; set; }
    public bool ResponseStatus { get; set; }
}
