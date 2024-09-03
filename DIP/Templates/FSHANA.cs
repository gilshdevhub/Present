using Newtonsoft.Json;

namespace DIP.Templates
{
    public class FSHANA
    {
        [JsonProperty(propertyName: "@Number")]
        public int Number { get; set; }
        [JsonProperty(propertyName: "@YEAR")]
        public int Year { get; set; }
        [JsonProperty(propertyName: "@MONTH")]
        public int Month { get; set; }
        [JsonProperty(propertyName: "@DATE")]
        public int Date { get; set; }
        [JsonProperty(propertyName: "@DAYONW")]
        public string DayOnw { get; set; }
        [JsonProperty(propertyName: "@DAYMIS")]
        public int DayMis { get; set; }
        [JsonProperty(propertyName: "@HAGNAM")]
        public string HagNam { get; set; }
        [JsonProperty(propertyName: "@CMO60")]
        public string CMO60 { get; set; }
    }
}
