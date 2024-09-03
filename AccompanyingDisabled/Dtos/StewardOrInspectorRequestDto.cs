using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace AccompanyingDisabled.Dtos;

public class StewardOrInspectorRequestDto
{
    [Required]
    public string MarsKey { get; set; }
    [Required]
    [Range(1, 99)]
    public int ResponseType { get; set; } 
    [Required]
    public int UserId { get; set; } 
}
