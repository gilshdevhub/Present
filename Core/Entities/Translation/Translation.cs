using Core.Entities.Configuration;
using Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Translation;

public class Translation
{
    public int Id { get; set; }
    [Key, Column(Order = 0)]
    public int SystemTypeId { get; set; }
    [Key, Column(Order = 1)]
    public string Key { get; set; }
    public string? Description { get; set; }
    public string? Hebrew { get; set; }
    public string? English { get; set; }
    public string? Russian { get; set; }
    public string? Arabic { get; set; }
    public SystemType SystemType { get; set; }
    public bool IsActive { get; set; }
}

public class TranslationRequest
{
    public SystemTypes SystemType { get; set; }
    public Languages? LanguageId { get; set; }
}
