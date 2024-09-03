using Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Dtos.PopupMessages;

public class PopupMessagesRequestDto
{
    [Required]
    [Range(minimum: 1, maximum: 2)]
    public PageTypes PageTypeId { get; set; }
    public SystemTypes? SystemTypeId { get; set; }
    [Required]
    [Range(minimum: 1, maximum: 4)]
    public Languages LanguageId { get; set; }
}
public class PopupMessagesStationRequestDto
{
    [Required]
    [Range(minimum: 1, maximum: 2)]
    public PageTypes PageTypeId { get; set; }
    public SystemTypes? SystemTypeId { get; set; }
    [Required]
    [Range(minimum: 1, maximum: 4)]
    public Languages LanguageId { get; set; }
    [JsonIgnore]
    public int? StationId { get; set; }
}


public class PopupMessagesResponseDto
{
    public int Id { get; set; }
    public int PageTypeId { get; set; }
    public string Title { get; set; }
    public string MessageBody { get; set; }
    public DateTime StartDate { get; set; }    public DateTime EndDate { get; set; }}
public class PopupMessagesWithStationsResponseDto
{
    public int Id { get; set; }
    public int PageTypeId { get; set; }
    public string Title { get; set; }
    public string MessageBody { get; set; }
    public DateTime StartDate { get; set; }    public DateTime EndDate { get; set; }    public string StationsIds { get; set; }

}
