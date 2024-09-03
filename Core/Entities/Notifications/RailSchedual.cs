namespace Core.Entities.Notifications;

public class RailSchedual
{
    public int Id { get; set; }
    public DateTime CreateTime { get; set; }
    public int TrainNumber { get; set; }
    public DateTime TrainDate { get; set; }
    public int StationOrder { get; set; }
    public int StationId { get; set; }
    public byte StopCode { get; set; }
    public int Platform { get; set; }
    public Nullable<DateTime> ArrivalTime { get; set; }
    public Nullable<DateTime> DepartureTime { get; set; }
}
