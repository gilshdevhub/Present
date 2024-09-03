namespace API.Dtos.Configurations;

public class WorkHoursDto
{
    public IEnumerable<WorkDayDto> workHours { get; set; }

}
public class WorkDayDto
{
    public string From { get; set; }
    public string To { get; set; }
}
