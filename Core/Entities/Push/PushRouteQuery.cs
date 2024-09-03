namespace Core.Entities.Push;

public class PushRouteQuery
{
    public Nullable<int> PushRegisterId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime TillDate { get; set; }
}
