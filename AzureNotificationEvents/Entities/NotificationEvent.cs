using System;

namespace AzureNotificationEvents.Entities
{
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
        public int Status { get; set; }
    }
}
