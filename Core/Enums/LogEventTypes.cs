using System.Text.Json.Serialization;

namespace Core.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LogEventTypes
{
    Add = 1,
    Update = 2,
    Delete = 3,
    Login=4
}