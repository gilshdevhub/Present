using Core.Entities.Configuration;

namespace Core.Entities.AppMessages;

public class PopUpMessages
{
    public int Id { get; set; }
    public int PageTypeId { get; set; }
    public string MessageBodyHebrew { get; set; }
    public string MessageBodyEnglish { get; set; }
    public string MessageBodyArabic { get; set; }
    public string MessageBodyRussian { get; set; }
    public string TitleHebrew { get; set; }
    public string TitleEnglish { get; set; }
    public string TitleArabic { get; set; }
    public string TitleRussian { get; set; }
    public DateTime StartDate { get; set; }    public DateTime EndDate { get; set; }    public PageType PageType { get; set; }
    public bool isActive { get; set; }
    public int? SystemTypeId { get; set; }
    public SystemType SystemType { get; set; }
    public string? StationsIds { get; set; }
}

public class MessageRequest
{
    public int PageTypeId { get; set; }
    public int? SystemTypeId { get; set; }
    public int? StationId { get; set; }
}
