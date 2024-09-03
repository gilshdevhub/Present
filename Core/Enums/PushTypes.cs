using System.Text.Json.Serialization;

namespace Core.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PushNotificationType
{
    OneTime = 1,
    Permanent = 2,
    All = 3
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PushNotificationState
{
    Active = 1,
    Canceled = 2,
    All = 3
}
