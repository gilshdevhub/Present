using Core.Entities.Vouchers;

namespace Core.Entities.Notifications;

public class AutomationNotification
{
    public int Id { get; set; }
    public DateTime CreateDate { get; set; }
    public int TrainNumber { get; set; }
    public DateTime TrainDate { get; set; }
    public int NotificationTypeId { get; set; }
    public int? ChangedStationId { get; set; }
    public int? ChangedPlatformId { get; set; }
    public DateTime? ChangedTrainTime { get; set; }
    public bool IsHandled { get; set; }

    public NotificationType NotificationType { get; set; }
    public required Station ChangedStation { get; set; }
}
