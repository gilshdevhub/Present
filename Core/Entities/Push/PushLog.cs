namespace Core.Entities.Push;

public class PushLogRequest
{
    public Nullable<int> PushRegistrationId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime TillDate { get; set; }
}
