using System.Text.Json.Serialization;

namespace Core.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SmsSubscriberType
{
    Inforu = 1,
    Telemassage = 2,
    UniCell = 3
}
