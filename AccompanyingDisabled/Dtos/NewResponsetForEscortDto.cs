using System.ComponentModel.DataAnnotations;

namespace AccompanyingDisabled.Dtos;

public class NewResponsetForEscortDto
{
    [Required]
    [Range(1, 9)]
    public int RequestStatus { get; set; }
    public string? StatusDiscription { get; set; }
}
