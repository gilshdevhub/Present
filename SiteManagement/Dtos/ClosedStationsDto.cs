using System.ComponentModel.DataAnnotations;

namespace SiteManagement.Dtos;

public class ClosedStationsAndLinesDto
{
    [Key]
    public int Id { get; set; }
    public int FromStation { get; set; }
    public int ToStation { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
}
