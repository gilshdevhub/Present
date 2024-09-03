using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Vouchers;

public class StationInfo
{
    public int StationInfoId { get; set; }
    public int StationId { get; set; }
    public decimal LinesMapsX { get; set; }
    public decimal LinesMapsY { get; set; }
    [ForeignKey("ParkingCosts")]
    public int ParkingCosts { get; set; }
    public bool BikeParking { get; set; }
    [ForeignKey("ParkingCosts")]
    public int BikeParkingCosts { get; set; }
    public bool AirPolution { get; set; }
    public string? StationMap { get; set; }
    public string? NonActiveElavators { get; set; }
    public bool? StationIsClosed { get; set; }
    public DateTime? StationIsClosedUntill { get; set; }
    public DateTime? StationInfoFromDate { get; set; }
    public DateTime? StationInfoToDate { get; set; }

}


public class StationInformationALL
{
    public StationInformationRsponseDto english { get; set; }
    public StationInformationRsponseDto russian { get; set; }
    public StationInformationRsponseDto hebrew { get; set; }
    public StationInformationRsponseDto arabic { get; set; }
}