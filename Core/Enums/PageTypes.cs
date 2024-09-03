using System.Text.Json.Serialization;

namespace Core.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PageTypes
{
    MainPage = 1,
    RoutePlanning = 2
}
