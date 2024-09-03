using System;
using System.Collections.Generic;
using System.Text;

namespace AzureAutomationNotifications.Entities
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

    public class NotificationEventResponse : ApiResponse
    {
        public bool Result { get; set; }
    }
}
