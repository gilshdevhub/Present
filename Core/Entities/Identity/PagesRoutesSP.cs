using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Identity;

public class PagesRoutesSP
{
    [Key]
    public bool readable { get; set; }
    public bool updatable { get; set; }
}

