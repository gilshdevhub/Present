using API.Dtos.Configurations;
using API.Dtos.Push;
using Core.Entities;
using Core.Entities.AppMessages;
using Core.Entities.Translation;
using Core.Entities.Vouchers;


namespace API.Dtos;

public class BackUpDto
{
    public IEnumerable<PopUpMessages>? popUpMessages { get; set; }
    public PushDto? pushNotifications { get; set; }
    public object? stationSVG { get; set; }
    public IEnumerable<StationAllDto>? stationResponseDto { get; set; }
    public IEnumerable<StationInformationALL>? stationInformation { get; set; }
    public IEnumerable<TendersDto>? tenders { get; set; }
    public IEnumerable<SingleSupplierDto>? singleSupplier { get; set; }
    public IEnumerable<TenderDocuments>?  tenderDocuments { get; set; }
    public IEnumerable<PlanningAndRatesDto>? planningAndRates {  get; set; }
    public IEnumerable<MeetingsDto>? meetings { get; set; }
    public IEnumerable<MailingList>? mailingLists { get; set; }
    public IEnumerable<ExemptionNoticesDto>? exemptionNotices { get; set; }
    public IEnumerable<Translation>? translations { get; set; }
    public IEnumerable<URLTranslation>? urlTranslation { get; set; }
    public IEnumerable<ConfigurationResponseDto>? configurationResponses { get; set; }
}

