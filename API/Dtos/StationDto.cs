using Core.Entities.Vouchers;
using Core.Enums;
using Core.Helpers.Validators;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos;

public class StationRequestDto
{
    [Required]
    [Range(minimum: 1, maximum: 4)]
    public Languages LanguageId { get; set; }
    [Required]
    [EnumRangeValidator]
    public SystemTypes SystemType { get; set; }
}

public class StationResponseDto
{
    public int StationId { get; set; }
    public string StationName { get; set; }
    public bool Parking { get; set; }
    public bool Handicap { get; set; }
    public StationLocation Location { get; set; }
    public IEnumerable<string> Synonyms { get; set; }
    public class StationLocation
    {
        public decimal Latitude { get; set; }
        public decimal Lontitude { get; set; }
    }
}
public class StationInfoResponseDto
{
    public int StationId { get; set; }
    public string StationName { get; set; }
    public bool Parking { get; set; }
    public bool Handicap { get; set; }
    public StationLocation Location { get; set; }
    public IEnumerable<string> Synonyms { get; set; }
    public StationInfo StationInfo { get; set; }
    public IEnumerable<StationInfoTranslation> StationInfoTranslation { get; set; }
    public class StationLocation
    {
        public decimal Latitude { get; set; }
        public decimal Lontitude { get; set; }
    }
}
public class StationAllDto
{
    public int StationId { get; set; }
    public int MetropolinId { get; set; }
    public int TicketStationId { get; set; }
    public string? RjpaName { get; set; }
    public string? HebrewName { get; set; }
    public string? EnglishName { get; set; }
    public string? RussianName { get; set; }
    public string? ArabicName { get; set; }
    public decimal Latitude { get; set; }
    public decimal Lontitude { get; set; }
    public bool Handicap { get; set; }
    public bool Parking { get; set; }
    public bool IsActive { get; set; }

}