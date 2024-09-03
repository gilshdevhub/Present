namespace Core.Helpers;

public static class ConfigurationKeys
{
    public static string AddSingleStopMassage => "NotificationAddSingleStopMassage";
    public static string AddMultiStopsMassage => "NotificationAddMultiStopsMassage";
    public static string CancelSingleStopMassage => "NotificationCancelSingleStopMassage";
    public static string CancelMultiStopsMassage => "NotificationCancelMultiStopsMassage";
    public static string CancelDepartureStationMassage => "NotificationCancelDepartureStationMassage";
    public static string CancelArrivalStationMassage => "NotificationCancelArrivalStationMassage";
    public static string OtpTimeOut => "OtpTimeOut";
    public static string CompensationOtpMessage => "CompensationOtpMessage";
    public static string OtpNumberOfDigits => "OtpNumberOfDigits";
    public static string ArrivalPlatformChangeMessage => "NotificationArrivalPlatformChangeMessage";
    public static string DeparturePlatformChangeMessage => "NotificationDeparturePlatformChangeMessage";
    public static string TrainDepartureDelay => "NotificationTrainDepartureDelayInMinutes";
    public static string TrainDepartureDelayMessage => "NotificationTrainDepartureDelayMessage";
    public static string TrainCancelMessage => "NotificationTrainCancelMessage";
}
