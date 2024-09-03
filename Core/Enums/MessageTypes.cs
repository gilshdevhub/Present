using System.Text.Json.Serialization;

namespace Core.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MessageTypes
{
    SMS = 1,
    Push = 2,
    Mail = 3,
    PushToAll = 4
}
