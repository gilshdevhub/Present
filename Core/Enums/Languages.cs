using System.Text.Json.Serialization;

namespace Core.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Languages
{
    Hebrew = 1,
    English = 2,
    Arabic = 3,
    Russian = 4
}
