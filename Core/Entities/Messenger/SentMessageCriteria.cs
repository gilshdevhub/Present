namespace Core.Entities.Messenger;

public class SentMessageCriteria
{
    public DateTime SentFromDate { get; set; }
    public DateTime SentTillDate { get; set; }
    public int MessageType { get; set; }
    public string SearchInfo { get; set; }
}
