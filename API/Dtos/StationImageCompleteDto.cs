using System.ComponentModel.DataAnnotations;

namespace API.Dtos;

public class StationImageCompleteDto
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Code { get; set; }
}
