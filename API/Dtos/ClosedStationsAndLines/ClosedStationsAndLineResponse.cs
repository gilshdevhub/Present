using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Dtos.ClosedStationsAndLines
{
    public class ClosedStationsAndLineResponse
    {
        public int CurrentVersion { get; set; }
        public List<ClosedStationsAndLinesOutDto> ClosedList { get; set; }

    }
    public class ClosedStationsAndLinesOutDto
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public int FromStation { get; set; }
        public int ToStation { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}
