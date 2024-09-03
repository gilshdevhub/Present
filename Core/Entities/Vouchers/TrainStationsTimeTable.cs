namespace Core.Entities.Vouchers;

public class TrainStationsTimeTable
{
    public int Id { get; set; }
    public int TrainTimeTableId { get; set; }
    public int TrainId { get; set; }
    public int StationSequence { get; set; }
    public int StationId { get; set; }
    public TimeSpan DepartureTime { get; set; }
    public TimeSpan ArrivalTime { get; set; }
    public DateTime TimeTableStartDate { get; set; }
    public DateTime TimeTableEndDate { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdated { get; set; }

    public required Station Station { get; set; }
    public TrainTimeTable TrainTimeTable { get; set; }
}
