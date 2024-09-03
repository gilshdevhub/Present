namespace Core.Entities.Push;

public class PushNotificationInfo : PushNotification
{
    public bool PermanentRegistration { get; set; }
    public int? SelectedDay { get; set; }
}
