using Core.Enums;

namespace Core.Entities.Notifications;

public class NotificationEvent
{
    public int Id { get; set; }
    public DateTime CreateDate { get; set; }
    public int PushNotificationId { get; set; }
    public int PushRegistrationId { get; set; }
    public int AutomationNotificationId { get; set; }
    public string Message { get; set; }
    public DateTime TimeToSend { get; set; }
    public int NotificationTypeId { get; set; }
    public NotificationStatus Status { get; set; }
    public NotificationType NotificationType { get; set; }
}

public class NotificationEventExtraInfo
{
    public string Ids { get; set; }
    public int AutomationNotificationId { get; set; }
    public int TrainNumber { get; set; }
    public int NotificationTypeId { get; set; }
    public string NotificationType { get; set; }
    public DateTime TrainDate { get; set; }
    public int StationId { get; set; }
    public string StationName { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime UpdateArrivalTime { get; set; }
    public DateTime TimeToSend { get; set; }
    public int? PlatformNumber { get; set; }
    public string Message { get; set; }
    public int StatusId { get; set; }
    public string StatusName { get; set; }
}
