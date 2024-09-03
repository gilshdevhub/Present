using Core.Entities.Notifications;
using Core.Entities.Push;
using Core.Enums;
using Core.Extensions;
using Core.Helpers;
using System.Text;

namespace Infrastructure.Services.Notifications;

public class TrainDelayNotification : BaseNotification
{
                      public override void ProcessNotification(IEnumerable<AutomationNotification> automationNotifications, PushNotification pushNotification)
    {
        try
        {
            Notification delayTrain = automationNotifications
                .Where(p => p.ChangedStationId == pushNotification.DepartureStationId)
                .OrderBy(p => p.Id)
                .Select(p => new Notification
                {
                    AutomationNotificationId = p.Id,
                    DepartureStationName = GetStationName(pushNotification.DepartureStationId),
                    OriginTime = pushNotification.DepartureTime,
                    TrainNumber = pushNotification.TrainNumber,
                    UpdatedTime = (DateTime)p.ChangedTrainTime,
                    TimeToSend = pushNotification.DepartureTime.AddMinutes(-30)
                }).LastOrDefault();

            if (delayTrain != null)
            {
                base.NotificationEvents.Add(new NotificationEvent
                {
                    Message = CreateMessage(pushNotification, delayTrain),
                    TimeToSend = delayTrain.TimeToSend,
                    PushNotificationId = pushNotification.Id,
                    NotificationTypeId = (int)NotificationTypes.TrainDelay,
                    AutomationNotificationId = delayTrain.AutomationNotificationId
                });
            }
        }
        catch (Exception ex)
        {
            throw new NotificationException("Notifications - Create notification message - Train Delay", $"push notification id: {pushNotification.Id}", ex);
        }
    }
    public override string CreateMessage(PushNotification pushNotification, Notification notification)
    {
        string messageFormat;
        StringBuilder message = new();

        messageFormat = GetConfigurationValue(ConfigurationKeys.TrainDepartureDelayMessage);
        _ = message.AppendFormat(messageFormat, notification.OriginTime.ToIsraelString(), notification.TrainNumber, notification.DepartureStationName, notification.UpdatedTime.ToString("HH:mm"));

        return message.ToString();
    }
}
