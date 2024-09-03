using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace AccompanyingDisabled.Dtos;

public class CustomerRequestDto
{
    [Required]
    public SystemTypes SystemTypeId { get; set; }
    [Required]
    [Range(0, 1)]
    public int ResponseType { get; set; } = 0;
    [Required]
    public string MarsKey { get; set; }
}
