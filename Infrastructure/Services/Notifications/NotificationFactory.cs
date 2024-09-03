using Core.Enums;

namespace Infrastructure.Services.Notifications;

public static class NotificationFactory
{
    public static BaseNotification Create(NotificationTypes notificationType)
    {
        BaseNotification baseNotification;

        switch (notificationType)
        {
            case NotificationTypes.AddTrainStop:
                baseNotification = new AddTrainStopNotification();
                break;
            case NotificationTypes.CancelTrainStop:
                baseNotification = new CancelTrainStopNotification();
                break;
            case NotificationTypes.PlatformChange:
                baseNotification = new PlatformChangeNotification();
                break;
            case NotificationTypes.TrainDelay:
                baseNotification = new TrainDelayNotification();
                break;
            case NotificationTypes.TrainCancel:
                baseNotification = new TrainCancelNotification();
                break;
            case NotificationTypes.UpdateTrainDelay:
                baseNotification = new UpdateTrainDelayNotification();
                break;
            default:
                throw new System.NotImplementedException();
        }

        return baseNotification;
    }
}
