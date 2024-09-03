using Core.Entities.Notifications;
using Core.Entities.Push;

namespace Infrastructure.Services.Notifications;

public interface IBaseNotification
{
    void ProcessNotification(IEnumerable<AutomationNotification> automationNotifications, PushNotification pushNotification);
    string CreateMessage(PushNotification pushNotification, Notification notification);
}
