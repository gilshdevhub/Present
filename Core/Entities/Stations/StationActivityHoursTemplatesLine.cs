using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Vouchers;

public class StationActivityHoursTemplatesLine
{
    public int Id { get; set; }
    [ForeignKey("StationActivityHoursTemplates")]
    public int TemplateId { get; set; }
    public string ActivityDaysNumbers { get; set; }
    public string StartHourPrefixTextKey { get; set; }
    public TimeSpan StartHour { get; set; }
    public string StartHourReplaceTextKey { get; set; }
    public string EndHourPrefixTextKey { get; set; }
    public TimeSpan EndHour { get; set; }
    public string EndHourReplaceTextKey { get; set; }
    public string EndHourPostfixTextKey { get; set; }
    public string ActivityHoursReplaceTextKey { get; set; }
    public StationActivityHoursTemplates? StationActivityHoursTemplates { get; set; }
}
