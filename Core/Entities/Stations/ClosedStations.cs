using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Stations;

public class ClosedStationsAndLines
{
    [Key]
    public int Id { get; set; }
    public int FromStation { get; set; }
    public int ToStation { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
}

