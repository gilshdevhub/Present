using System.Collections.ObjectModel;

namespace Core.Entities.Push;

public class PushRouting
{
    public PushRouting()
    {
        this.PushNotificationsByDate = new Collection<PushNotificationsByDate>();
        this.PushNotificationsByWeekDay = new Collection<PushNotificationsByWeekDay>();
    }

    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public int State { get; set; }
    public int PermanentRegistration { get; set; }
    public int PushRegistrationId { get; set; }

    public PushRegistration PushRegistration { get; set; }
    public ICollection<PushNotificationsByDate> PushNotificationsByDate { get; set; }
    public ICollection<PushNotificationsByWeekDay> PushNotificationsByWeekDay { get; set; }
}
