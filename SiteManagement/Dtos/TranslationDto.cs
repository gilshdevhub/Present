using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace SiteManagement.Dtos;

public class TranslationRequestDto
{
    [Required]
    public SystemTypes SystemTypeId { get; set; }
    [Required]
    [MaxLength(60)]
    public string Key { get; set; }
    [MaxLength(120)]
    public string Description { get; set; }
    [MaxLength(2048)]
    public string Hebrew { get; set; }
    [MaxLength(2048)]
    public string English { get; set; }
    [MaxLength(2048)]
    public string Russian { get; set; }
    [MaxLength(2048)]
    public string Arabic { get; set; }
    [Required]
    public bool IsActive { get; set; }
}

public class TranslationResponseDto
{
    public int Id { get; set; }
    public int SystemTypeId { get; set; }
    public string Key { get; set; }
    public string? Description { get; set; }
    public string Hebrew { get; set; }
    public string? English { get; set; }
    public string? Russian { get; set; }
    public string? Arabic { get; set; }
    public bool IsActive { get; set; }
}
