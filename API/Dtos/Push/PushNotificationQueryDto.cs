using API.Helpers.Validators;
using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Push;

[PushNotificationSearchValidator]
public class PushNotificationRequestQueryDto
{
    [Required]
    public int PushRegistrationId { get; set; }
    public PushNotificationType? NotificationType { get; set; }
    public PushNotificationState? NotificationState { get; set; }
}

public class PushNotificationResponseQueryDto
{
    public int PushRoutingId { get; set; }
    public int NotificationType { get; set; }
    public int NotificationState { get; set; }
    public IEnumerable<int> WeekDays { get; set; }
    public IEnumerable<TrainRouteInfo> TrainRouteInfos { get; set; }
}

public class TrainRouteInfo
{
    public int TrainNumber { get; set; }
    public DateTime DepartureTime { get; set; }    public DateTime ArrivalTime { get; set; }      public int DepartureStationId { get; set; }    public int ArrivalStationId { get; set; }  }
