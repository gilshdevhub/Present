using System.Text.Json.Serialization;

namespace Core.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ScheduleTypes
{
    ByDeparture = 1,
    ByArrival = 2
}
