using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace AccompanyingDisabled.Dtos;

public class GetEscortLuzRequestDto
{
    [Required]
    public SystemTypes SystemTypeId { get; set; }
    [Required]
    public string UserId { get; set; }
    [Required]
    public DateTime? FromDate { get; set; }=DateTime.Now;
    [Required]
    public DateTime? ToDate { get; set; }
    [Required]
    [Range(1, 2)]
    public int RecipientType { get; set; }
}
