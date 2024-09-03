using System.ComponentModel.DataAnnotations.Schema;
namespace Core.Entities.Messenger;

public class SentMessages
{
    public SentMessages(byte messageType, string messageTypeInfo, string responseStatus, DateTime sentDate/*, byte state*/)
    {
        MessageType = messageType;
        MessageTypeInfo = messageTypeInfo;
               ResponseStatus = responseStatus;
        SentDate = sentDate;
           }
    public int Id { get; set; }
    public byte MessageType { get; set; }
    public string MessageTypeInfo { get; set; }
    [ForeignKey("Message")]
    public int MessageId { get; set; }
    public string ResponseStatus { get; set; }
    public DateTime SentDate { get; set; }
    public byte State { get; set; }
    public Message Message { get; set; }
}
public class SentMessageResponse
{
    public int Id { get; set; }
    public int MessageType { get; set; }
    public string MessageTypeInfo { get; set; }
    public string Message { get; set; }
    public string ResponseStatus { get; set; }
    public DateTime SentDate { get; set; }
    public int State { get; set; }
}

