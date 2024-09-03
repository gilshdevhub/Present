namespace Core.Entities.Vouchers;

public class TrainStationVoucher
{
    public int Id { get; set; }
    public int StationSequence { get; set; }
    public int StationId { get; set; }
    public TimeSpan DepartureTime { get; set; }
    public TimeSpan ArrivalTime { get; set; }
    public int NOV { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdated { get; set; }

    public required Station Station { get; set; }
    public TrainVoucher TrainVoucher { get; set; }
}
