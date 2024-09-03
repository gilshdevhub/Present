using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Stations;

public class StationImageNamesInit
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string ElementKey { get; set; }
    [Required]
    public string siStationmInit { get; set; }
}
