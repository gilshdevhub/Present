using Core.Entities.Notifications;
using Core.Entities.Push;
using Core.Enums;
using Core.Extensions;
using Core.Helpers;
using System.Text;

namespace Infrastructure.Services.Notifications;

public class PlatformChangeNotification : BaseNotification
{
                      public override void ProcessNotification(IEnumerable<AutomationNotification> automationNotifications, PushNotification pushNotification)
    {
        try
        {
            #region שינוי רציף בתחנת עליה
            IEnumerable<AutomationNotification> departurePlatformChanges = automationNotifications.Where(p => p.ChangedStationId == pushNotification.DepartureStationId)
                .OrderBy(p => p.Id).ToArray();

            int platformNumber = GetTrainPlatform(pushNotification.TrainDate.Date, pushNotification.TrainNumber, pushNotification.DepartureStationId);

            if (departurePlatformChanges.Any() && (departurePlatformChanges.Last().ChangedPlatformId != platformNumber))
            {
                Notification platformChange = new()
                {
                    DepartureStationName = GetStationName(pushNotification.DepartureStationId),
                    OriginTime = pushNotification.DepartureTime,
                    TrainNumber = pushNotification.TrainNumber,
                    ChangedPlatformId = (int)departurePlatformChanges.Last().ChangedPlatformId,
                    TimeToSend = pushNotification.DepartureTime.AddMinutes(-5),
                    AutomationNotificationId = departurePlatformChanges.Last().Id
                };

                base.NotificationEvents.Add(new NotificationEvent
                {
                    Message = CreateMessage(pushNotification, platformChange),
                    TimeToSend = platformChange.TimeToSend,
                    NotificationTypeId = (int)NotificationTypes.PlatformChange,
                    PushNotificationId = pushNotification.Id,
                    AutomationNotificationId = platformChange.AutomationNotificationId
                });
            }
            #endregion

            #region שינוי רציף בתחנת ירידה
            IEnumerable<AutomationNotification> arrivalPlatformChanges = automationNotifications.Where(p => p.ChangedStationId == pushNotification.ArrivalStationId)
                .OrderBy(p => p.Id).ToArray();

            platformNumber = GetTrainPlatform(pushNotification.TrainDate.Date, pushNotification.TrainNumber, pushNotification.ArrivalStationId);

            if (arrivalPlatformChanges.Any() && (arrivalPlatformChanges.Last().ChangedPlatformId != platformNumber))
            {
                Notification platformChange = new()
                {
                    ArrivalStationName = GetStationName(pushNotification.ArrivalStationId),
                    OriginTime = pushNotification.ArrivalTime,
                    TrainNumber = pushNotification.TrainNumber,
                    ChangedPlatformId = (int)arrivalPlatformChanges.Last().ChangedPlatformId,
                    TimeToSend = pushNotification.ArrivalTime.AddMinutes(-5),
                    AutomationNotificationId = arrivalPlatformChanges.Last().Id
                };

                base.NotificationEvents.Add(new NotificationEvent
                {
                    Message = CreateMessage(pushNotification, platformChange),
                    TimeToSend = platformChange.TimeToSend,
                    NotificationTypeId = (int)NotificationTypes.PlatformChange,
                    PushNotificationId = pushNotification.Id,
                    AutomationNotificationId = platformChange.AutomationNotificationId
                });
            }
            #endregion
        }
        catch (Exception ex)
        {
            throw new NotificationException("Notifications - Create notification message - Platform Change", $"push notification id: {pushNotification.Id}", ex);
        }
    }
    public override string CreateMessage(PushNotification pushNotification, Notification notification)
    {
        string messageFormat;
        StringBuilder message = new();

        if (!string.IsNullOrEmpty(notification.ArrivalStationName))
        {
            messageFormat = GetConfigurationValue(ConfigurationKeys.ArrivalPlatformChangeMessage);
            _ = message.AppendFormat(messageFormat, notification.TrainNumber, notification.OriginTime.ToIsraelString(), notification.ArrivalStationName, notification.ChangedPlatformId);
        }
        else
        {
            messageFormat = GetConfigurationValue(ConfigurationKeys.DeparturePlatformChangeMessage);
            _ = message.AppendFormat(messageFormat, notification.OriginTime.ToIsraelString(), notification.DepartureStationName, notification.TrainNumber, notification.ChangedPlatformId);
        }

        return message.ToString();
    }
}
