using System.ComponentModel.DataAnnotations;

namespace AccompanyingDisabled.Dtos;

public class CustomerResponseDto
{
    [Required]
    [Range(1, 2)]
    public int RequestStatus { get; set; } = 0;
    [Required]
    public string StatusDiscription { get; set; } 
    [Required]
    public string StatusCode { get; set; }
    [Required]
    public string Data { get; set; }
}
