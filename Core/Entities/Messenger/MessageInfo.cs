using Core.Enums;
using Microsoft.AspNetCore.Http;
using SendGrid.Helpers.Mail;

namespace Core.Entities.Messenger;

public class MessageInfo
{
    public MessageInfo()
    {
    }

    public MessageInfo(string[] keys, string Message)
    {
        this.Keys = keys;
        this.Message = Message;
    }
    public MessageInfo(string key, string Message) : this(new string[] { key }, Message)
    {
        this.Message = Message;
    }

    public string Message { get; set; }
    public string[] Keys { get; set; }
    public MessageTypes MessageType { get; set; }
    public string? Subject { get; set; }
}

public class MailInfo
{
                     
    public string Message { get; set; }
    public List<EmailAddress> Addresses { get; set; }
    public string Subject { get; set; }
      }


public class SmsInfo
{
    public string Message { get; set; }
    public string Number { get; set; }

}

public class SendMail
{
    public IFormFile FileToLoad { get; set; }
    public int DocType { get; set; }
    public string DocDisplay { get; set; }

}
public class SendFirebaseMessagesAsyncDto
{
    public SendFirebaseMessagesAsyncDto(MessageInfo MessageInfo, List<string> TokenIds)
    {
        messageInfo = MessageInfo;
        tokenIds = TokenIds;
    }

    public MessageInfo messageInfo { get; set; }
    public List<string> tokenIds { get; set; }
 }

public class MessageTextDTO
{
    public string messageText { get; set; }
}

public class SendFirebaseMessagesDto
{
    public SendFirebaseMessagesDto(string Message, List<string> TokenIds)//, string Subject)
    {
        message = Message;
        tokenIds = TokenIds;
         }

    public string message { get; set; }
    public List<string> tokenIds { get; set; }
   
}
