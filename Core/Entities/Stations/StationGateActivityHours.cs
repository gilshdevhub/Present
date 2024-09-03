using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Vouchers;

public class StationGateActivityHours
{
    public int StationHoursId { get; set; }
    [ForeignKey("StationGate")]
    public int StationGateId { get; set; }
    [ForeignKey("StationActivityTemplatesTypes")]
    public int TemplateTypeId { get; set; }
    public bool IsClosed { get; set; }
    public DateTime? ClosedUntill { get; set; }
    public StationActivityTemplatesTypes StationActivityTemplatesTypes { get; set; }
    public StationGate StationGate { get; set; }
}

public class StationGateActivityHoursDto
{
    public int StationHoursId { get; set; }
    public int StationGateId { get; set; }
    public int TemplateTypeId { get; set; }
    public bool IsClosed { get; set; }
    public DateTime? ClosedUntill { get; set; }
    public int StationHoursLineId { get; set; }
    public string ActivityDaysNumbers { get; set; }
    public string StartHourPrefixTextKey { get; set; }
    public TimeSpan StartHour { get; set; }
    public string StartHourReplaceTextKey { get; set; }
    public string EndHourPrefixTextKey { get; set; }
    public TimeSpan EndHour { get; set; }
    public string EndHourReplaceTextKey { get; set; }
    public string EndHourPostfixTextKey { get; set; }
    public string ActivityHoursReplaceTextKey { get; set; }
}

public class StationGateActivityHoursByGateDto
{
    public Dictionary<int, List<StationGateActivityHoursDto>> StationGateActivityHours { get; set; }
    public string NonActiveElavators { get; set; }
}

public class StationGateActivityHoursPostDto
{
    public HoursPostDto Hours { get; set; }
    public IEnumerable<StationGateActivityHoursLinePostDto> StationGateActivityHoursLineDto { get; set; }
}

public class StationGateActivityHoursPutDto
{
    public HoursDto Hours { get; set; }
    public IEnumerable<StationGateActivityHoursLineDto> StationGateActivityHoursLineDto { get; set; }
}

public class HoursDto
{
    public int StationHoursId { get; set; }
    public int StationGateId { get; set; }
    public int TemplateTypeId { get; set; }
    public bool IsClosed { get; set; }
    public DateTime ClosedUntill { get; set; }
}
public class HoursPostDto
{
    public int StationGateId { get; set; }
    public int TemplateTypeId { get; set; }
    public bool IsClosed { get; set; }
    public DateTime ClosedUntill { get; set; }
}
public class StationGateActivityHoursLineDto
{
    public int Id { get; set; }
    public int StationHoursId { get; set; }
    public string ActivityDaysNumbers { get; set; }
    public string StartHourPrefixTextKey { get; set; }
    public string StartHour { get; set; }
    public string StartHourReplaceTextKey { get; set; }
    public string EndHourPrefixTextKey { get; set; }
    public string EndHour { get; set; }
    public string EndHourReplaceTextKey { get; set; }
    public string EndHourPostfixTextKey { get; set; }
    public string ActivityHoursReplaceTextKey { get; set; }
}
public class StationGateActivityHoursLinePostDto
{
    public string ActivityDaysNumbers { get; set; }
    public string StartHourPrefixTextKey { get; set; }
    public string StartHour { get; set; }
    public string StartHourReplaceTextKey { get; set; }
    public string EndHourPrefixTextKey { get; set; }
    public string EndHour { get; set; }
    public string EndHourReplaceTextKey { get; set; }
    public string EndHourPostfixTextKey { get; set; }
    public string ActivityHoursReplaceTextKey { get; set; }
}
public class GateNamesDto
{
    public string name { get; set; }
}
