using System.Text.Json.Serialization;

namespace AzureCacheForUmbraco.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CacheKeys
    {
        He = 9,
        EnUs = 10,
        Ar = 11,
        Ru = 12 
    }
}
