namespace Core.Entities.Vouchers;

public class DailyTrainStationsTimeTable
{
    public int Id { get; set; }
    public DateTime TrainDate { get; set; }
    public int StationSequence { get; set; }
    public int StationId { get; set; }
    public TimeSpan DepartureTime { get; set; }
    public TimeSpan ArrivalTime { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdated { get; set; }

    public required Station Station { get; set; }
    public DailyTrainsTimeTable DailyTrainsTimeTable { get; set; }
}
