namespace Core.Entities.Push;

public class PushNotificationsByWeekDay
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public int TrainNumber { get; set; }
    public int DepartureStationId { get; set; }    public int ArrivalStationId { get; set; }      public int DepartutePlatform { get; set; }
    public int ArrivalPlatform { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public bool day1 { get; set; }
    public bool day2 { get; set; }
    public bool day3 { get; set; }
    public bool day4 { get; set; }
    public bool day5 { get; set; }
    public bool day6 { get; set; }
    public bool day7 { get; set; }
    public int PushRoutingId { get; set; }
    public PushRouting PushRouting { get; set; }

}
