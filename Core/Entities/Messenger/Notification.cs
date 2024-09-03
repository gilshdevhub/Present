namespace Core.Entities.Messenger;

public class Notification
{
    public string send_date { get; set; } = "now";
    public string content { get; set; } = "test";
    public string wp_type { get; set; } = "Toast";
    public int wp_count { get; set; } = 3;
    public string android_delivery_priority { get; set; } = "high";
    public IEnumerable<string> devices { get; set; }
}
