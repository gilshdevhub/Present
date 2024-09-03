namespace Core.Entities.Messenger;

public class Message
{
    public int Id { get; set; }
    public string MessageText { get; set; }
    public IEnumerable<SentMessages> newSentMessages { get; set; }
}