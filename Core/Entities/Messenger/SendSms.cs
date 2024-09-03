namespace Core.Entities.Messenger;

public class SendSms
{
    public string Recipients { get; set; }
    public string MessageSubject { get; set; }
    public string MessageBody { get; set; }
    public int Priority { get; set; }
    public bool SaveToLog { get; set; }
}
