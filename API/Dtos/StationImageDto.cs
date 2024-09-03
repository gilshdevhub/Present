using System.ComponentModel.DataAnnotations;

namespace API.Dtos;

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

}