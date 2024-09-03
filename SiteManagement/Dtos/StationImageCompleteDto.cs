using System.ComponentModel.DataAnnotations;

namespace SiteManagement.Dtos;

public class StationImageCompleteDto
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Code { get; set; }
}
