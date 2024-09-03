using System.Collections.ObjectModel;

namespace Core.Entities.Notifications;

public class NotificationType
{
    public NotificationType()
    {
        this.AutomationNotifications = new Collection<AutomationNotification>();
        this.NotificationEvents = new Collection<NotificationEvent>();
    }
    public int Id { get; set; }
    public string Description { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    public ICollection<AutomationNotification> AutomationNotifications { get; set; }
    public ICollection<NotificationEvent> NotificationEvents { get; set; }
}
