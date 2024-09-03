namespace Core.Entities.Push;

public class PushNotificationsByDate
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public int TrainNumber { get; set; }
    public DateTime DepartureTime { get; set; }    public DateTime ArrivalTime { get; set; }      public int DepartureStationId { get; set; }    public int ArrivalStationId { get; set; }      public int DepartutePlatform { get; set; }
    public int ArrivalPlatform { get; set; }
    public DateTime TrainDate { get; set; }    public int PushRoutingId { get; set; }

    public PushRouting PushRouting { get; set; }
}
