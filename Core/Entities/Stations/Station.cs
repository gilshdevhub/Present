using Core.Entities.Notifications;
using Core.Entities.RailUpdates;
using Core.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Vouchers;

public class Station
{
    public Station()
    {
        this.ArrivalTrainTimeTable = new Collection<TrainTimeTable>();
        this.DepartureTrainTimeTable = new Collection<TrainTimeTable>();
        this.TrainStationsTimeTable = new Collection<TrainStationsTimeTable>();
        this.ArrivalDailyTrainTimeTable = new Collection<DailyTrainsTimeTable>();
        this.DepartureDailyTrainTimeTable = new Collection<DailyTrainsTimeTable>();
        this.DailyTrainStationsTimeTable = new Collection<DailyTrainStationsTimeTable>();
        this.ArrivalTrainVoucher = new Collection<TrainVoucher>();
        this.DepartureTrainVoucher = new Collection<TrainVoucher>();
        this.TrainStationVoucher = new Collection<TrainStationVoucher>();
        this.Synonym = new Collection<Synonym>();
        this.ChangedStationAutomationNotification = new Collection<AutomationNotification>();
           }

    public int StationId { get; set; }
    public int MetropolinId { get; set; }
    public int TicketStationId { get; set; }
    public string? RjpaName { get; set; }
    public string? HebrewName { get; set; }
    public string? EnglishName { get; set; }
    public string? RussianName { get; set; }
    public string? ArabicName { get; set; }
    public decimal Latitude { get; set; }
    public decimal Lontitude { get; set; }
    public bool Handicap { get; set; }
    public bool Parking { get; set; }
    public bool IsActive { get; set; }

    public Metropolin Metropolin { get; set; }
    public ICollection<TrainTimeTable> ArrivalTrainTimeTable { get; set; }
    public ICollection<TrainTimeTable> DepartureTrainTimeTable { get; set; }
    public ICollection<TrainStationsTimeTable> TrainStationsTimeTable { get; set; }
    public ICollection<DailyTrainsTimeTable> ArrivalDailyTrainTimeTable { get; set; }
    public ICollection<DailyTrainsTimeTable> DepartureDailyTrainTimeTable { get; set; }
    public ICollection<DailyTrainStationsTimeTable> DailyTrainStationsTimeTable { get; set; }
    public ICollection<TrainVoucher> ArrivalTrainVoucher { get; set; }
    public ICollection<TrainVoucher> DepartureTrainVoucher { get; set; }
    public ICollection<TrainStationVoucher> TrainStationVoucher { get; set; }
    public ICollection<Synonym>? Synonym { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public ICollection<AutomationNotification> ChangedStationAutomationNotification { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public ICollection<Compensation.Compensation> CompensationOriginStation { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public ICollection<Compensation.Compensation> CompensationDestinationStation { get; set; }
    public StationInfo StationInfo { get; set; }
    public ICollection<StationInfoTranslation> StationInfoTranslation { get; set; }
    public ICollection<StationGate> StationGate { get; set; }
}


public class StationInformationRequestDto
{
    [Required]
    [Range(minimum: 1, maximum: 4)]
    public Languages LanguageId { get; set; }
    [Required]
    public int SystemType { get; set; }
    [Required]
    public int StationId { get; set; }
}

public class ClosedStationsDto
{
    [Required]
    public decimal Latitude { get; set; }
    [Required]
    public decimal Lontitude { get; set; }
    [Required]
    public double distance { get; set; }
}

public class StationInformationRsponseDto
{
    public DateTime creationDate { get; set; }
    public string? Version { get; set; }
    public int successStatus { get; set; }
    public int statusCode { get; set; }
    public string? errorMessages { get; set; }
    public List<RailUpdateResponseUmbracoDto> StationUpdates { get; set; }                                                                                public StationDetails StationDetails { get; set; }
    public List<GateInfo> GateInfo { get; set; }
    public IEnumerable<string> EasyCategories { get; set; }
    public IEnumerable<string> SafetyInfos { get; set; }

}
public class StationUpdate
{
          public StationUpdate(string updateHeader, string updateLink)
    {
        UpdateHeader = updateHeader;
        UpdateLink = updateLink;
    }
    public StationUpdate(StationUpdate result)
    {
        UpdateHeader = result.UpdateHeader;
        UpdateLink = result.UpdateLink;
    }
    public string? UpdateHeader { get; set; }
    public string? UpdateLink { get; set; }
}


public class StationDetails
{
    public int stationId { get; set; }
    public string? stationName { get; set; }
    public string? CarParking { get; set; }
    public string? ParkingCosts { get; set; }
    public string? BikeParking { get; set; }
    public string? BikeParkingCosts { get; set; }
    public string? AirPollutionIcon { get; set; }
    public string? StationMap { get; set; }
    public string? NonActiveElevators { get; set; }
    public string? NonActiveElevatorsLink { get; set; }
    public bool StationIsClosed { get; set; }
    public DateTime? StationIsClosedUntill { get; set; }
    public string? StationIsClosedText { get; set; }
    public string? StationInfoTitle { get; set; }
    public string? StationInfo { get; set; }
    public string? AboutStationTitle { get; set; }
    public string? AboutStationContent { get; set; }
    public string? ParkingTitleTranslationKey { get; set; }
    public string? ParkingContentTranslationKey { get; set; }
}
public class GateInfo
{
    public int StationGateId { get; set; }
    public string? GateName { get; set; }
    public string? GateAddress { get; set; }
    public decimal GateLatitude { get; set; }
    public decimal GateLontitude { get; set; }
    public List<GateActivityHours> GateActivityHours { get; set; }
    public List<GateServices> GateServices { get; set; }
}
public class GateActivityHours
{
    public int ActivityHoursType { get; set; }
    public string? IsClosedShortText { get; set; }
    public string? IsClosedLongText { get; set; }
    public string? ActivityDaysNumbers { get; set; }
    public string? StartHourTextKey { get; set; }
    public string? StartHour { get; set; }
    public string? StartHourReplaceTextKey { get; set; }
    public string? EndHourPrefixTextKey { get; set; }
    public string? EndHour { get; set; }
    public string? EndHourReplaceTextKey { get; set; }
    public string? EndHourPostfixTextKey { get; set; }
    public string? ActivityHoursReplaceTextKey { get; set; }
}

public class GateServices
{
    public string? ServiceName { get; set; }
    public string? ServiceIcon { get; set; }
    public string? ServiceIconLink { get; set; }
}

public class InfoDto
{
    public string? ParkCosts { get; set; }
    public string? BikeParkCosts { get; set; }
    public bool BikeParking { get; set; }
    public bool AirPollution { get; set; }
    public string? StationMap { get; set; }
    public string? NonActiveElevators { get; set; }
    public bool? StationIsClosed { get; set; }
    public DateTime? StationIsClosedUntill { get; set; }
    public string? StationInfoTitleTranslationKey { get; set; }
}
