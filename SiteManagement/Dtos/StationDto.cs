using Core.Entities.Vouchers;
using System.ComponentModel.DataAnnotations;

namespace SiteManagement.Dtos;

public class StationDto
{
    public int StationId { get; set; }
    public int MetropolinId { get; set; }
    public int TicketStationId { get; set; }
    [MaxLength(60)]
    public string? HebrewName { get; set; }
    [MaxLength(60)]
    public string? EnglishName { get; set; }
    [MaxLength(60)]
    public string? RussianName { get; set; }
    [MaxLength(60)]
    public string? ArabicName { get; set; }
    public decimal Latitude { get; set; }
    public decimal Lontitude { get; set; }
    public bool Handicap { get; set; }
    public bool Parking { get; set; }
    public bool IsActive { get; set; }
    public IEnumerable<Synonym> Synonyms { get; set; }

    public StationInfo StationInfo { get; set; }
    public bool AirPolution { get; set; }
    public int ParkingCosts { get; set; }
    public bool BikeParking { get; set; }
    public int BikeParkingCosts { get; set; }
    public string? StationMap { get; set; }
    public string? NonActiveElavators { get; set; }
    public bool StationIsClosed { get; set; }
    public decimal LinesMapsX { get; set; }
    public decimal LinesMapsY { get; set; }
    public DateTime StationInfoFromDate { get; set; }
    public DateTime StationInfoToDate { get; set; }
    public IEnumerable<StationInfoTranslation> StationInfoTranslation { get; set; }
    public IEnumerable<StationGate>? StationGate { get; set; }
}
public class StationUpdateDto
{
    public int StationId { get; set; }
    public int MetropolinId { get; set; }
    public int TicketStationId { get; set; }
    [MaxLength(60)]
    public string? HebrewName { get; set; }
    [MaxLength(60)]
    public string? EnglishName { get; set; }
    [MaxLength(60)]
    public string? RussianName { get; set; }
    [MaxLength(60)]
    public string? ArabicName { get; set; }
    public decimal Latitude { get; set; }
    public decimal Lontitude { get; set; }
    public bool Handicap { get; set; }
    public bool Parking { get; set; }
    public bool IsActive { get; set; }
    public IEnumerable<Synonym> Synonyms { get; set; }

    public StationInfo StationInfo { get; set; }
    public bool AirPolution { get; set; }
    public int ParkingCosts { get; set; }
    public bool BikeParking { get; set; }
    public int BikeParkingCosts { get; set; }
    public string? StationMap { get; set; }
    public string? NonActiveElavators { get; set; }
    public bool StationIsClosed { get; set; }
    public decimal LinesMapsX { get; set; }
    public decimal LinesMapsY { get; set; }
    public DateTime StationInfoFromDate { get; set; }
    public DateTime StationInfoToDate { get; set; }
    public IEnumerable<StationInfoTranslation> StationInfoTranslation { get; set; }
//      public IEnumerable<StationGate> StationGate { get; set; }
}
public class StationElevatorsDto
{
    public string? nonActiveElavators { get; set; }
    public int stationId { get; set; }
}
    public class StationInfoDto
{
    public bool AirPolution { get; set; }
    public string? ParkingCosts { get; set; }
    public bool BikeParking { get; set; }
    public string? BikeParkingCosts { get; set; }
    public string? StationMap { get; set; }
    public string? NonActiveElavators { get; set; }
    public bool StationIsClosed { get; set; }
    public decimal LinesMapsX { get; set; }
    public decimal LinesMapsY { get; set; }
}
public class StationInfoTranslationDto
{
       public DateTime? StationInfoFromDate { get; set; }
    public DateTime? StationInfoToDate { get; set; }
    public int StationId { get; set; }
}
public class SynonymDto
{
    public int LanguageId { get; set; }
    public string? SynonymName { get; set; }
}
public class DeleteStationDto
{
    public int StationId { get; set; }
}

//public class StationGateDto
//{
//    public bool GateClosed { get; set; }
//    public DateTime GateClosedUntill { get; set; }
//    public decimal GateLatitude { get; set; }
//    public decimal GateLontitude { get; set; }
//    public string? GateName { get; set; }
//    public string? GateAddress { get; set; }
//    public int GateOrder { get; set; }
//    public bool GateParking { get; set; }
//    public int StationGateId { get; set; }
//    public int StationId { get; set; }
//    public string? StationName { get; set; }
//}
