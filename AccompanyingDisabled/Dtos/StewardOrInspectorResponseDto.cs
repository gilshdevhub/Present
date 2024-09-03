using System.ComponentModel.DataAnnotations;

namespace AccompanyingDisabled.Dtos;

public class StewardOrInspectorResponseDto
{
    [Required]
    [Range(1, 2)]
    public int RequestStatus { get; set; } = 0;
    [Required]
    public string StatusDiscription { get; set; } = "יש תקלה במערכת חייג לשירות הלקוחות *2560";
    [Required]
    public string StatusCode { get; set; }

}
