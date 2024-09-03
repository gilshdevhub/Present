using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Stations;

public class StationImage
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string ElementKey { get; set; }
    [Required]
    public string ElementName { get; set; }
    [Required]
    public string ElementCode { get; set; }
    public string StationmInit { get; set; }
    [Required]
    public bool IsInactive { get; set; }
    [Required]
    public DateTime FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public DateTime? LastUpdated { get; set; }
    [Required]
    public bool BetweenDatesInactive { get; set; }
    public string? ElementCodeHE { get; set; }
    public string? ElementCodeEN { get; set; }
    public string? ElementCodeRU { get; set; }
    public string? ElementCodeAR { get; set; }

}
public class UpdateElement
{
    public StationImage element { get; set; }
    public int setInActive { get; set; }
}
