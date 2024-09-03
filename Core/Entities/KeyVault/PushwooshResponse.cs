namespace Core.Entities.Messenger;

public class PushwooshResponse
{
    public int status_code { get; set; }
    public string status_message { get; set; }
    public MessageResponse response { get; set; }
}

public class MessageResponse
{
    public IEnumerable<string> Messages { get; set; }
}
