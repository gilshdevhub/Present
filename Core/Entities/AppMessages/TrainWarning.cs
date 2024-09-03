using Core.Entities.Configuration;

namespace Core.Entities.AppMessages;

public class TrainWarning
{
    public int Id { get; set; }
    public int TrainNumber { get; set; }
    public string MessageBodyEnglish { get; set; }
    public string MessageBodyArabic { get; set; }
    public string MessageBodyRussian { get; set; }
    public string MessageBodyHebrew { get; set; }
    public string TitleHebrew { get; set; }
    public string TitleEnglish { get; set; }
    public string TitleArabic { get; set; }
    public string TitleRussian { get; set; }
    public DateTime StartDate { get; set; }    public DateTime EndDate { get; set; }    public bool isActive { get; set; }
    public int? StartStation { get; set; }
    public int? EndStation { get; set; }
    public SystemType SystemType { get; set; }
    public int? SystemTypeId { get; set; }
    public WarningType WarningType { get; set; }
    public int WarningTypeId { get; set; }

}