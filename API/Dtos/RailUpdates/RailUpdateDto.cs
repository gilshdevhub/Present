using Core.Enums;
using Core.Helpers.Validators;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos.RailUpdates;

public class RailUpdateRequestDto
{
    [Required]
    [Range(minimum: 1, maximum: 4)]

    public Languages LanguageId { get; set; }
    [Required]
    [EnumRangeValidator]
    public SystemTypes SystemType { get; set; }
}

public class RailUpdateRequestbyStationIdDto
{
    [Required]
    [Range(minimum: 1, maximum: 4)]

    public Languages LanguageId { get; set; }
    [Required]
    [EnumRangeValidator]
    public SystemTypes SystemType { get; set; }
    [Required]
    public int StationId { get; set; }
}

public class RailUpdateResponseDto
{
    public string UpdateHeader { get; set; }
    public string UpdateContent { get; set; }
    public string UpdateLink { get; set; }
    public string Image { get; set; }
    public bool IsFloat { get; set; }
    public required IEnumerable<string> Stations { get; set; }

    public void Update(string updateHeader, string updateContent, string updateLink)
    {
        this.UpdateContent = updateContent;
        this.UpdateHeader = updateHeader;
        this.UpdateLink = updateLink;
    }
}
