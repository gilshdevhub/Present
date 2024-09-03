using Core.Enums;

namespace Core.Entities.Push;

public class PushNotificationQuery
{
    public int PushRegistrationId { get; set; }
    public PushNotificationType? NotificationType { get; set; }
    public PushNotificationState? NotificationState { get; set; }
         }


public class PushNotificationResponse
{
    public List<PushNotificationsByWeekDayResponse> pushNotificationsByWeekDayResponse { get; set; }
    public List<PushNotificationsByDateResponse> pushNotificationsByDateResponse { get; set; }
}
public class PushNotificationsByWeekDayResponse
{
    public DateTime DepartureTime { get; set; }    public DateTime ArrivalTime { get; set; }      public int DepartureStationId { get; set; }    public int ArrivalStationId { get; set; }      public List<PushNotificationsByWeekDay> PushNotificationsByWeekDay { get; set; }
}

public class PushNotificationsByDateResponse
{
    public DateTime DepartureTime { get; set; }    public DateTime ArrivalTime { get; set; }      public int DepartureStationId { get; set; }    public int ArrivalStationId { get; set; }      public List<PushNotificationsByDate> PushNotificationsByDate { get; set; }
}
