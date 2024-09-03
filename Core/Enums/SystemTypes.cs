using System.Text.Json.Serialization;

namespace Core.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SystemTypes
{
    Mobile = 1,
    Web = 2,
    Kiosk = 3,
    Minisite = 4,
    BLS= 5,
   
}
