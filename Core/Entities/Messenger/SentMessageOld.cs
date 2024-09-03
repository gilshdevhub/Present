namespace Core.Entities.Messenger;

public class SentMessageOld
{
    public int Id { get; set; }
    public int MessageType { get; set; }
    public string MessageTypeInfo { get; set; }
    public string Message { get; set; }
    public string ResponseStatus { get; set; }
    public DateTime SentDate { get; set; }
    public int State { get; set; }
}
