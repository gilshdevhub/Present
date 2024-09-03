using System.ComponentModel.DataAnnotations;

namespace SiteManagement.Dtos;

public class StationImageDto
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
}
public class ImageElementsDto
{
    public int Id { get; set; }
    public string ElementKey { get; set; }
    public string ElementName { get; set; }
    public bool IsInactive { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public DateTime? LastUpdated { get; set; }
    public bool BetweenDatesInactive { get; set; }
}
public class StationPathDeactivateDto {
    public int elementId { get; set; } 
    public bool closed{ get; set; } 
    public DateTime fromdate{ get; set; }
    public DateTime? todate{ get; set; }
    public bool betweenDatesInactive { get; set; }
}