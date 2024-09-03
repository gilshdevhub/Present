namespace Infrastructure.Services.Notifications;

public class Notification
{
    public int TrainNumber { get; set; }
    public string StationName { get; set; }
    public string MassageType { get; set; }
    public DateTime OriginTime { get; set; }
    public DateTime TimeToSend { get; set; }
    public DateTime UpdatedTime { get; set; }
    public int ChangedPlatformId { get; set; }
    public string ArrivalStationName { get; set; }
    public string DepartureStationName { get; set; }
    public int AutomationNotificationId { get; set; }
}
