namespace SiteManagement.Dtos;

public class TrainWarningRequestDto
{
    public int Id { get; set; }
    public int TrainNumber { get; set; }
    public string MessageBodyHebrew { get; set; }
    public string MessageBodyEnglish { get; set; }
    public string MessageBodyArabic { get; set; }
    public string MessageBodyRussian { get; set; }
    public string TitleHebrew { get; set; }
    public string TitleEnglish { get; set; }
    public string TitleArabic { get; set; }
    public string TitleRussian { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool isActive { get; set; }
    public int? StartStation { get; set; }
    public int? EndStation { get; set; }
    public int? SystemTypeId { get; set; }
    public int WarningTypeId { get; set; }
}

public class TrainWarningResponseDto
{
    public int Id { get; set; }
    public int TrainNumber { get; set; }
    public string MessageBodyHebrew { get; set; }
    public string MessageBodyEnglish { get; set; }
    public string MessageBodyArabic { get; set; }
    public string MessageBodyRussian { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool isActive { get; set; }
    public int? StartStation { get; set; }
    public int? EndStation { get; set; }
    public string SystemTypeName { get; set; }
    public int? SystemTypeId { get; set; }
    public string TitleHebrew { get; set; }
    public string TitleEnglish { get; set; }
    public string TitleArabic { get; set; }
    public string TitleRussian { get; set; }
    public int WarningTypeId { get; set; }
    public string WarningTypeName { get; set; }
}