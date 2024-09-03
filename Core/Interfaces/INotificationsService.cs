using Core.Entities.Notifications;

namespace Core.Interfaces;

public interface INotificationsService
{
    Task<IEnumerable<NotificationType>> GetNotificationTypesAsync();
    Task<int> ProcessAutomationNotificationAsync(IEnumerable<AutomationNotification> automationNotifications);
    Task<bool> ProcessNotificationEventsAsync(IEnumerable<NotificationEvent> notificationEvents);
    Task<IEnumerable<NotificationEventExtraInfo>> GetNotificationEventExtraInfoAsync();
    Task<bool> UpdateNotificationEventsStatus(IEnumerable<NotificationEvent> notificationEvents);
}
