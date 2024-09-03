using System.ComponentModel.DataAnnotations;

namespace SiteManagement.Dtos;

public class PopUpMessagesRequestDto 
{
    public int Id { get; set; }
    [Required]
    public int PageTypeId { get; set; }
    [MaxLength(8192)]
    public string MessageBodyHebrew { get; set; }
    [MaxLength(8192)]
    public string MessageBodyEnglish { get; set; }
    [MaxLength(8192)]
    public string MessageBodyArabic { get; set; }
    [MaxLength(8192)]
    public string MessageBodyRussian { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool isActive { get; set; }
    public int? SystemTypeId { get; set; }
    [MaxLength(120)]
    public string TitleHebrew { get; set; }
    [MaxLength(120)]
    public string TitleEnglish { get; set; }
    [MaxLength(120)]
    public string TitleArabic { get; set; }
    [MaxLength(120)]
    public string TitleRussian { get; set; }
    [MaxLength(350)]
    public string? StationsIds { get; set; }

}

public class PopUpMessagesResponseDto
{
    public int Id { get; set; }
    [Required]
    public int PageTypeId { get; set; }
    public string PageTypeName { get; set; }
    public string MessageBodyHebrew { get; set; }
    public string MessageBodyEnglish { get; set; }
    public string MessageBodyArabic { get; set; }
    public string MessageBodyRussian { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool isActive { get; set; }
    public string SystemTypeName { get; set; }
    public int? SystemTypeId { get; set; }
    public string TitleHebrew { get; set; }
    public string TitleEnglish { get; set; }
    public string TitleArabic { get; set; }
    public string TitleRussian { get; set; }
    public string? StationsIds { get; set; }

}