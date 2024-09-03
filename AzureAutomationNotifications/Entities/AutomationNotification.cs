using System;
using System.Collections.Generic;
using System.Text;

namespace AzureAutomationNotifications.Entities
{
    public class AutomationNotification
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int TrainNumber { get; set; }
        public DateTime TrainDate { get; set; }
        public int NotificationTypeId { get; set; }
        public Nullable<int> ChangedStationId { get; set; }
        public Nullable<int> ChangedPlatformId { get; set; }
        public Nullable<DateTime> ChangedTrainTime { get; set; }
        public bool IsHandled { get; set; }
    }

    public class AutomationNotificationResponse : ApiResponse
    {
        public int Result { get; set; }
    }
}