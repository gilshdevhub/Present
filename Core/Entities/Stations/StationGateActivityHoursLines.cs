using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Vouchers;

public class StationGateActivityHoursLines
{
    public int Id { get; set; }
    [ForeignKey("StationGateActivityHours")]
    public int StationHoursId { get; set; }
    public string ActivityDaysNumbers { get; set; }
    public string StartHourPrefixTextKey { get; set; }
    public TimeSpan StartHour { get; set; }
    public string StartHourReplaceTextKey { get; set; }
    public string EndHourPrefixTextKey { get; set; }
    public TimeSpan EndHour { get; set; }
    public string EndHourReplaceTextKey { get; set; }
    public string EndHourPostfixTextKey { get; set; }
    public string ActivityHoursReplaceTextKey { get; set; }
    public StationGateActivityHours? StationGateActivityHours { get; set; }
}
