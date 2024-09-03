using Core.Entities.Messenger;
using Core.Enums;

namespace Core.Interfaces;

public interface IMessengerService
{
    Task<bool> SendTelemessageAsync(MessageInfo messageInfo);
    Task<InforuResponse> SendInforuSmsAsync(SendSms messageInfo);
    Task<bool> SendUniCellSmsAsync(MessageInfo messageInfo);
    string SendEmail();
    Task<bool> SendPushAsync(MessageInfo messageInfo);
    Task<IEnumerable<SentMessageResponse>> GetSentMessagesAsync(SentMessageCriteria request);
    Task<IEnumerable<int>> GetTrains();
    Task<IEnumerable<MaslulResponse>> GetStationsIDs(StationsRequest stationsRequest);
    Task<SendGrid.Response> MailSendingTender(MailInfo messageInfo, string? mailsender);
    Task<SendFirebaseMessagesAsyncDto> CountPushWithFirebaseAsync(MessageInfo messageInfo);
    Task<int> CountPushToAllAsync();
    Task<bool> SendFirebaseMessagesAsync(SendFirebaseMessagesAsyncDto sendFirebaseMessagesAsyncDto);
    Task<bool> SendPushToAllMessagesAsync(string messageText);
    
    Task<string> CheckButtomState(MessageTypes buttonType);
    Task<SendGrid.Response> MailSendingAttachment(MailInfo messageInfo, string? mailSender, SendGrid.Helpers.Mail.Attachment? fileAttachment);
}
