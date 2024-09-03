using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DIP.Templates
{
    public class FTHNOT
    {
        [JsonProperty(propertyName: "@THANA")]
        public int StationId { get; set; }
        [JsonProperty(propertyName: "@THTEUR")]
        public string RjpaName { get; set; }
    }
}
