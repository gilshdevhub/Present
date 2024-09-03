namespace Core.Entities.Vouchers;

public class StationGate
{
    public int StationGateId { get; set; }
    public int StationId { get; set; }
    public bool GateParking { get; set; }
    public string? GateNameTranslationKey { get; set; }
    public string? GateAddressTranslationKey { get; set; }
    public decimal GateLatitude { get; set; }
    public decimal GateLontitude { get; set; }
    public int GateOrder { get; set; }
    public bool GateClosed { get; set; }
    public DateTime GateClosedUntill { get; set; }
    public ICollection<StationGateServices> StationGateServices { get; set; }
      }
public class StationGateDto
{
    public bool GateClosed { get; set; }
    public DateTime GateClosedUntill { get; set; }
    public decimal GateLatitude { get; set; }
    public decimal GateLontitude { get; set; }
    public string? GateNameHebrew { get; set; }
    public string? GateAddressHebrew { get; set; }
    public int GateOrder { get; set; }
    public bool GateParking { get; set; }
    public int StationGateId { get; set; }
    public int StationId { get; set; }
    public string? StationName { get; set; }
    public string? GateNameEnglish { get; set; }
    public string? GateAddressEnglish { get; set; }
    public string? GateNameArabic { get; set; }
    public string? GateAddressArabic { get; set; }
    public string? GateNameRussian { get; set; }
    public string? GateAddressRussian { get; set; }
       public StationInfoTranslation GateName { get; set; }
    public StationInfoTranslation GateAddress { get; set; }
    public string? GateNameTranslationKey { get; set; }
    public string? GateAddressTranslationKey { get; set; }
    public ICollection<StationGateServices> StationGateServices { get; set; }
}
public class StationGateRequestDto
{
    public int StationGateId { get; set; }
    public int StationId { get; set; }
    public bool GateParking { get; set; }
    public string? GateNameTranslationKey { get; set; }
    public string? GateAddressTranslationKey { get; set; }
    public decimal GateLatitude { get; set; }
    public decimal GateLontitude { get; set; }
    public int GateOrder { get; set; }
    public bool GateClosed { get; set; }
    public DateTime GateClosedUntill { get; set; }
    public ICollection<StationGateServices> StationGateServices { get; set; }
          public StationInfoTranslation[] StationInfoTranslation { get; set; }

}

public class StationGateRequestInsertDto
{
       public int StationId { get; set; }
    public bool GateParking { get; set; }
    public string? GateNameTranslationKey { get; set; }
    public string? GateAddressTranslationKey { get; set; }
    public decimal GateLatitude { get; set; }
    public decimal GateLontitude { get; set; }
    public int GateOrder { get; set; }
    public bool GateClosed { get; set; }
    public DateTime GateClosedUntill { get; set; }
             public StationInfoTranslation[] StationInfoTranslation { get; set; }

}

public class StationGateServicesDto
{
       public int StationGateId { get; set; }
    public int ServiceId { get; set; }
    public bool isServiceExist { get; set; }
    public string? ServiceName { get; set; }
}