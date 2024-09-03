using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Vouchers;

public class RailCalendar
{
    public int Id { get; set; }
    [Key]
    public DateTime Date { get; set; }
    public string DayInWeek { get; set; }
    public int NumberOfDayInWeek { get; set; }
    public string NameOfHoliday { get; set; }
    public string CMO60 { get; set; }
}
