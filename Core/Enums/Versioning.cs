using System.Text.Json.Serialization;

namespace Core.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Versioning
{
    Stations = 1,
    Configurations = 2,
    Translations = 3,
	PopUpMessages=4,
	URLTranslations=5,
    ClosedStationsAndLines=6
}
