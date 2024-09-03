namespace Core.Entities.Messenger;

public class PushwooshMessage
{
    public string application { get; set; }
    public string auth { get; set; }
    public List<Notification> notifications { get; set; }
}
