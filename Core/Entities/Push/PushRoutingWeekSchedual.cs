namespace Core.Entities.Push;

public class PushRoutingWeekSchedual
{
    public int Id { get; set; }
    public int PushRoutingId { get; set; }
    public int WeekDay { get; set; }

    public PushRouting PushRouting { get; set; }
}
