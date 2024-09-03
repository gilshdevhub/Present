using Core.Config;
using Core.Entities;
using Core.Entities.Configuration;
using Core.Entities.Messenger;
using Core.Entities.Push;
using Core.Entities.UniCell;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security;
using System.Text;
using System.Web;
using System.Xml;

namespace Infrastructure.Services;

public class MessengerService : IMessengerService
{
    private readonly IConfiguration _configuration;
    private readonly RailDbContext _context;
    private readonly ILogger<MessengerService> _logger;
    private readonly IOptions<PushwooshConfig> _pushwooshConfigOptions;
    private readonly IOptions<TelemassageConfig> _telemessageConfigOptions;
       private readonly IMailService _mailService;
    private readonly IConfigurationService _configurationService;
    private readonly IHttpClientService _httpClient;
    private readonly IOptions<APIMailSettingsConfig> _apiMailSettingsConfig;
    private readonly IManagmentSystemObjects _managmentSystemObjects;
    private readonly int maxCount;
    private readonly int pushDelayMilisec;

    private enum PushNotificationType
    {
        Pushwoosh = 1,
        Firebase = 2
    }

    public MessengerService(IConfiguration configuration, RailDbContext context, ILogger<MessengerService> logger, IOptions<PushwooshConfig> pushwooshConfigOptions,
        IOptions<TelemassageConfig> telemessageConfigOptions, IMailService mailService, IConfigurationService configurationService,
        IHttpClientService httpClient, IOptions<APIMailSettingsConfig> apiMailSettingsConfig, IManagmentSystemObjects managmentSystemObjects)//, IOptions<UniCellConfig> uniCellConfig)
    {
        _configuration = configuration;
        _context = context;
        _logger = logger;
        _pushwooshConfigOptions = pushwooshConfigOptions;
        _telemessageConfigOptions = telemessageConfigOptions;
        _mailService = mailService;
        _configurationService = configurationService;
               _httpClient = httpClient;
        _apiMailSettingsConfig = apiMailSettingsConfig;
        _managmentSystemObjects = managmentSystemObjects;
        maxCount = 400;// int.Parse((_configurationService.GetItemAsync("MaxBulkPushMembers", SystemTypes.Mobile).Result.Value));
        pushDelayMilisec = 2000;// int.Parse((_configurationService.GetItemAsync("MaxBulkPushDelay", SystemTypes.Mobile).Result.Value));
    }

    #region Send messages
    public string SendEmail()
    {
        throw new NotImplementedException();
    }

    public async Task<InforuResponse> SendInforuSmsAsync(SendSms messageInfo)
    {
        Assembly asm = Assembly.GetExecutingAssembly();
        string resourceName = "Infrastructure.Templates.InforuMessage.xml";

        XmlDocument xmlDocument = new();
        using (Stream stream = asm.GetManifestResourceStream(resourceName))
        {
            using XmlTextReader reader = new(stream);
            xmlDocument.Load(reader);
        }

        string xmlTemplate = xmlDocument.InnerXml;
        xmlTemplate = xmlTemplate.Replace("{USER_NAME}", SecurityElement.Escape(_configuration.GetSection("SMS:Inforu:UserName").Value));
        xmlTemplate = xmlTemplate.Replace("{API_TOKEN}", SecurityElement.Escape(_configuration.GetSection("SMS:Inforu:ApiToken").Value));
        xmlTemplate = xmlTemplate.Replace("{MESSAGE_BODY}", SecurityElement.Escape(messageInfo.MessageBody));
        xmlTemplate = xmlTemplate.Replace("{RECIPIENTS}", SecurityElement.Escape(messageInfo.Recipients));
        xmlTemplate = HttpUtility.UrlEncode(xmlTemplate, Encoding.UTF8);

               xmlTemplate = xmlTemplate.Replace(" ", "+");
        string result = await PostMessageAsync(_configuration.GetSection("SMS:Inforu:ApiUrl").Value, "InforuXML=" + xmlTemplate, _configuration.GetSection("SMS:Inforu:ContentType").Value);

        XmlDocument xml = GetXml(result);
        string json = JsonConvert.SerializeXmlNode(xml);

        InforuResponse inforuResponse = JsonConvert.DeserializeObject<InforuResponse>(json);
        return inforuResponse;
    }
    public async Task<bool> SendUniCellSmsAsync(MessageInfo messageInfo)
    {
        UniCellSendData uniCellSendData = new();
        List<UniCellRecipient> uniCellRecipients = new();

        uniCellSendData.UserName = _configuration.GetSection("SMS:UniCell:UserName").Value;
        uniCellSendData.Password = _configuration.GetSection("SMS:UniCell:Password").Value;
        uniCellSendData.SenderName = _configuration.GetSection("SMS:UniCell:SenderName").Value;
        uniCellSendData.RootReference = int.Parse(_configuration.GetSection("SMS:UniCell:RootReference").Value);
        uniCellSendData.Ist2s = bool.Parse(_configuration.GetSection("SMS:UniCell:Ist2s").Value);
        uniCellSendData.Relative = int.Parse(_configuration.GetSection("SMS:UniCell:Relative").Value);

        uniCellSendData.BodyMessage = SecurityElement.Escape(messageInfo.Message);
        string[] temp = messageInfo.Keys[0].Split(',');
        foreach (var item in temp)
        {
            UniCellRecipient recp = new();
            recp.Cellphone = item;
            uniCellRecipients.Add(recp);
        }
        uniCellSendData.Recipients = uniCellRecipients;

        UniCellResponse response = new();
        try
        {
            string data = await _httpClient.GetBodyInfoAsync(_configuration.GetSection("SMS:UniCell:ApiUrl").Value,
                uniCellSendData, "application/json");
            response = JsonConvert.DeserializeObject<UniCellResponse>(data);
            getKeys(temp, messageInfo);
            await SaveSentMessagesNewAsync(messageInfo, data, MessageTypes.SMS).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error for SendUniCellSmsAsync", ex);
        }

        return response.StatusCode == 0;
    }

    public async Task<bool> SendPushAsync(MessageInfo messageInfo)
    {
        bool result = false;

        try
        {
            Configuration notificationType = await _configurationService.GetItemAsync("NotificationType", SystemTypes.Mobile).ConfigureAwait(false);
            switch ((PushNotificationType)int.Parse(notificationType.Value))
            {
                case PushNotificationType.Pushwoosh:
                    result = await SendPushWithPushwooshAsync(messageInfo).ConfigureAwait(false);
                    break;
                case PushNotificationType.Firebase:
                    result = await SendPushWithFirebaseAsync(messageInfo).ConfigureAwait(false);
                    break;
                default:
                    _logger.LogWarning("Unknown push notification type!");
                                       break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Error for SendPushAsync", ex);
                   }

        return result;
    }

    public async Task<bool> SendTelemessageAsync(MessageInfo messageInfo)
    {
        Assembly asm = Assembly.GetExecutingAssembly();
        string resourceName = "Infrastructure.Templates.Telemessage.xml";

        XmlDocument xmlDocument = new();
        using (Stream stream = asm.GetManifestResourceStream(resourceName))
        {
            using XmlTextReader reader = new(stream);
            xmlDocument.Load(reader);
        }

        string xmlTemplate = xmlDocument.InnerXml;
        xmlTemplate = xmlTemplate.Replace("{USER_NAME}", SecurityElement.Escape(_telemessageConfigOptions.Value.UserName));
        xmlTemplate = xmlTemplate.Replace("{PASSWORD}", SecurityElement.Escape(_telemessageConfigOptions.Value.Password));
        xmlTemplate = xmlTemplate.Replace("{MESSAGE_SUBJECT}", messageInfo.Subject);
        xmlTemplate = xmlTemplate.Replace("{MESSAGE_BODY}", SecurityElement.Escape(messageInfo.Message));

        StringBuilder recipientsXML = new();
        resourceName = "Infrastructure.Templates.TelemessageRecipient.xml";
        using (Stream stream = asm.GetManifestResourceStream(resourceName))
        {
            using XmlTextReader reader = new(stream);
            xmlDocument.Load(reader);
        }
        string messageRecipient = xmlDocument.InnerXml;

        string[] recipients = messageInfo.Keys;
        foreach (string recipient in recipients)
        {
            string recipientType = messageInfo.MessageType == MessageTypes.SMS ? "SMS" : "EmailAddress";
            _ = recipientsXML.Append(messageRecipient.Replace("{RECIPIENT_TYPE}", recipientType).Replace("{RECIPIENT_INFO}", recipient));
        }

        xmlTemplate = xmlTemplate.Replace("{RECIPIENTS}", recipientsXML.ToString());
        string result = await PostMessageAsync(_configuration.GetSection("SMS:Telemassage:ApiUrl").Value, xmlTemplate, _configuration.GetSection("SMS:Telemassage:ContentType").Value)
            .ConfigureAwait(false);

        XmlDocument xml = GetXml(result);
        string json = JsonConvert.SerializeXmlNode(xml.SelectSingleNode("TELEMESSAGE"));
        await SaveSentMessagesAsync(messageInfo, json, messageInfo.MessageType).ConfigureAwait(false);

        TelemessageResponse telemessageResponse = JsonConvert.DeserializeObject<TelemessageResponse>(json);

        return telemessageResponse.Telemessage.Telemessage_Content.Response.Response_Status == 100;
    }
    #endregion

    #region Sent messages
    public async Task<IEnumerable<SentMessageResponse>> GetSentMessagesAsync(SentMessageCriteria request)
    {
        IQueryable<SentMessages> sentMessages = _context.SentMessages
            .Where(p => p.SentDate >= request.SentFromDate && p.SentDate <= request.SentTillDate && p.State == (int)RegistrationState.Signed);

        if (request.MessageType > 0)
        {
            sentMessages = sentMessages.Where(p => p.MessageType == request.MessageType);
        }

        if (request.SearchInfo != null)
        {
            sentMessages = sentMessages
            .Where(p => p.MessageTypeInfo == request.SearchInfo);
        }

        IEnumerable<SentMessageResponse> response = await sentMessages.Join(_context.Message,
                                       x => x.MessageId,
                                       y => y.Id,
                                       (x, y) => new SentMessageResponse
                                       {
                                           Id = x.Id,
                                           Message = y.MessageText,
                                           MessageType = x.MessageType,
                                           MessageTypeInfo = x.MessageTypeInfo,
                                           ResponseStatus = x.ResponseStatus,
                                           SentDate = x.SentDate,
                                           State = x.State
                                       }
                                       ).ToArrayAsync().ConfigureAwait(false);



        return response;
    }
                        
                        
          #endregion

    public async Task<IEnumerable<int>> GetTrains()
    {
        IEnumerable<int> trains = await _context.RailScheduals.Select(m => m.TrainNumber).Distinct().OrderBy(x => x).ToArrayAsync().ConfigureAwait(false);
        return trains;
    }

    public async Task<IEnumerable<MaslulResponse>> GetStationsIDs(StationsRequest stationsRequest)
    {
        IEnumerable<MaslulResponse> stations =
            await _context.RailScheduals
            .Join(_context.Stations, a => a.StationId, b => b.StationId, (a, b) => new { a.StationId, a.TrainNumber, b.HebrewName, a.StopCode, a.StationOrder, a.TrainDate })
            .Where(x => x.TrainNumber == stationsRequest.Id && x.StopCode == 1 && x.TrainDate.Date == stationsRequest.RequestedDate.Date)
                       .Select(m => new MaslulResponse { StationId = m.StationId, HebrewName = m.HebrewName, StationOrder = m.StationOrder })
            .Distinct()
            .OrderBy(x => x.StationOrder)
            .ToArrayAsync().ConfigureAwait(false);
        return stations;
    }
    private async Task<bool> SendPushWithPushwooshAsync(MessageInfo messageInfo)
    {
        IEnumerable<string> tokenIds = await _context.PushRegistrations.Where(p => messageInfo.Keys.Contains(p.Id.ToString())).Select(p => p.TokenId)
            .ToArrayAsync().ConfigureAwait(false);

        PushwooshMessage pushwooshmessage = new();
        pushwooshmessage.auth = _pushwooshConfigOptions.Value.Token;
        pushwooshmessage.application = _pushwooshConfigOptions.Value.Application;

        List<Notification> notifications = new();
        notifications.Add(new Notification() { content = messageInfo.Message, devices = tokenIds });
        pushwooshmessage.notifications = notifications;

        PushwooshRequest request = new() { request = pushwooshmessage };
        string json = System.Text.Json.JsonSerializer.Serialize<PushwooshRequest>(request);

        string postMessageResult = await PostMessageAsync(_pushwooshConfigOptions.Value.ApiUrl, json, _pushwooshConfigOptions.Value.ContentType).ConfigureAwait(false);

        if (!string.IsNullOrEmpty(postMessageResult))
        {
            await SaveSentMessagesAsync(messageInfo, postMessageResult, MessageTypes.Push).ConfigureAwait(false);
            PushwooshResponse response = System.Text.Json.JsonSerializer.Deserialize<PushwooshResponse>(postMessageResult);
            return response.status_code == (int)System.Net.HttpStatusCode.OK;
        }
        else
        {
            return false;
        }
    }
    private async Task<bool> SendPushWithFirebaseAsync(MessageInfo messageInfo)
    {
        bool isSuccess = false;

        List<string> tokenIds = new();
        string[] temp;

        List<PushRegistration> activeTokenIds = await _context.PushRegistrations.Where(p => p.State == (int)RegistrationState.Signed).ToListAsync();

        if (messageInfo.MessageType == MessageTypes.Push)
        {
            string[] tempKeys = messageInfo.Keys[0].Split(",");
            tokenIds = activeTokenIds.Where(p => tempKeys.Contains(p.Id.ToString()) && p.State == (int)RegistrationState.Signed)
                .Select(p => p.TokenId).ToList();
            temp = activeTokenIds.Where(p => tempKeys.Contains(p.Id.ToString()) && p.State == (int)RegistrationState.Signed)
                .Select(p => p.Id.ToString()).ToArray();
            getKeys(temp, messageInfo);
        }
        else if (messageInfo.MessageType == MessageTypes.PushToAll)
        {
            tokenIds = activeTokenIds.Select(p => p.TokenId).ToList();
            temp = activeTokenIds.Select(p => p.Id.ToString()).ToArray();
            getKeys(temp, messageInfo);
        }
        SendFirebaseMessagesAsyncDto sendFirebaseMessagesAsyncDto = new(messageInfo, tokenIds);

        isSuccess = await SendFirebaseMessagesAsync(sendFirebaseMessagesAsyncDto).ConfigureAwait(false);
        return isSuccess;
    }
    public async Task<SendFirebaseMessagesAsyncDto> CountPushWithFirebaseAsync(MessageInfo messageInfo)
    {
        List<string> tokenIds = new();
        string[] temp;

        List<PushRegistration> activeTokenIds = await _context.PushRegistrations.Where(p => p.State == (int)RegistrationState.Signed).ToListAsync();

        string[] tempKeys = messageInfo.Keys[0].Split(",");
        tokenIds = activeTokenIds.Where(p => tempKeys.Contains(p.Id.ToString()) && p.State == (int)RegistrationState.Signed)
            .Select(p => p.TokenId).ToList();
        temp = activeTokenIds.Where(p => tempKeys.Contains(p.Id.ToString()) && p.State == (int)RegistrationState.Signed)
            .Select(p => p.Id.ToString()).ToArray();
        getKeys(temp, messageInfo);

        SendFirebaseMessagesAsyncDto res = new(messageInfo, tokenIds);
        return res;
    }

    public async Task<int> CountPushToAllAsync()
    {

        int res = await _context.PushRegistrations.Where(p => p.State == (int)RegistrationState.Signed).CountAsync();
        return res;
    }
    public async Task<bool> SendPushToAllMessagesAsync(string messageText)
    {
        bool isSuccess = false;
        _ = await _managmentSystemObjects.UpdateManagmentObjectsByIdAsync(2, "false");

        List<string> tokenIds = await _context.PushRegistrations.Where(p => p.State == (int)RegistrationState.Signed).Select(p => p.TokenId).ToListAsync();
        isSuccess = await Sending(tokenIds, messageText, "PushToAll");

        _ = await _managmentSystemObjects.UpdateManagmentObjectsByIdAsync(2, "true");

        return isSuccess;

    }
    public async Task<bool> SendFirebaseMessagesAsync(SendFirebaseMessagesAsyncDto sendFirebaseMessagesAsyncDto)
    {
        bool isSuccess = false;
        if (sendFirebaseMessagesAsyncDto.tokenIds.Any())
        {

            _ = await _managmentSystemObjects.UpdateManagmentObjectsByIdAsync(1, "false");

           
            isSuccess = await Sending(sendFirebaseMessagesAsyncDto.tokenIds, sendFirebaseMessagesAsyncDto.messageInfo.Message, "Push");
           
            
        }
        _ = await _managmentSystemObjects.UpdateManagmentObjectsByIdAsync(1, "true");

        return isSuccess;
    }

    private async Task<bool> Sending (List<string> tokenIds, string Message, string Type)
    {
        string[] responseStatuses = Array.Empty<string>();
        bool isSuccess = false;
        int index = 0;

        var listTokenIds = new List<List<string>>();

        for (int i = 0; i < tokenIds.Count; i += maxCount)
        {
            listTokenIds.Add(tokenIds.GetRange(i, Math.Min(maxCount, tokenIds.Count - i)));
        }
        for (int i = 0; i < listTokenIds.Count; i++)
        {
            List<string>? tokenIdsBulk = listTokenIds[i];
            FirebaseAdmin.Messaging.MulticastMessage messages = new()
            {
                Tokens = tokenIdsBulk.ToArray(),
                Notification = new FirebaseAdmin.Messaging.Notification { Body = Message }
            };

            FirebaseAdmin.Messaging.BatchResponse result = await FirebaseAdmin.Messaging.FirebaseMessaging.DefaultInstance.SendMulticastAsync(messages).ConfigureAwait(false);


            responseStatuses = result.Responses.Select(p => $"Successful: {p.IsSuccess} Message Id: {p.MessageId} Exception Message: {(p.Exception != null ? p.Exception.Message : null)}").ToArray();

            isSuccess = result.SuccessCount > 0;
            index++;

            await Task.Delay(pushDelayMilisec);
        }
        await SaveSentMessagesNewAsync(new MessageInfo() { Message=Message }, responseStatuses[0], MessageTypes.Push).ConfigureAwait(false);
        return isSuccess;
    }
    public async Task<string> CheckButtomState(MessageTypes buttonType)
    {
        IEnumerable<ManagmentSystemObjects> managmentSystemObjects = await _managmentSystemObjects.GetManagmentObjectsAsync().ConfigureAwait(false);
        string buttonState = string.Empty;// new();
        switch (buttonType)
        {
            case MessageTypes.Push:
                buttonState = (managmentSystemObjects.SingleOrDefault(x => x.Id == 1)).StringValue;
                break;
            case MessageTypes.PushToAll:
                buttonState = (managmentSystemObjects.SingleOrDefault(x => x.Id == 2)).StringValue;
                break;
                                                     }
               return buttonState;
    }
    private void getKeys(string[] temp, MessageInfo messageInfo)
    {
        int k = 0;
        string temp2 = "";
        List<string> temp3 = new();
        for (int i = 0; i < temp.Length; i++)
        {
            if (i % maxCount == 0 && i > 0)
            {
                temp3.Add(temp2);
                k++;
            }

            if (i % maxCount == 0)
            {
                temp2 = temp[i];
            }
            else
            {
                temp2 += "," + temp[i];
            }
        }
        temp3.Add(temp2);
        messageInfo.Keys = temp3.ToArray();
    }

    private async Task<string> PostMessageAsync(string url, string data, string contentType)
    {
        string result = string.Empty;

        WebRequest request = WebRequest.Create(url);
        request.Timeout = 30000;
        request.Method = "POST";
        request.ContentType = contentType;

        try
        {
                       byte[] postBuffer = Encoding.UTF8.GetBytes(data);
            request.ContentLength = postBuffer.Length;
                       using Stream requestStream = request.GetRequestStream();
                       await requestStream.WriteAsync(postBuffer, 0, postBuffer.Length).ConfigureAwait(false);

                       requestStream.Close();
                       WebResponse response = await request.GetResponseAsync().ConfigureAwait(false);

                       using StreamReader streamReader = new(response.GetResponseStream(), Encoding.UTF8);
                       result = await streamReader.ReadToEndAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PostMessageAsync Failed", $"url: {url}, data: {data}");
        }

        return result;
    }
    private static XmlDocument GetXml(string xmlData)
    {
        XmlDocument xmlDocument = new();
        xmlDocument.LoadXml(xmlData);
        return xmlDocument;
    }

    private async Task SaveSentMessagesNewAsync(MessageInfo messageInfo, string responseStatuse, MessageTypes messageTypes)
    {
        try
        {
            Message sentMessages = new()
            {
                MessageText = messageInfo.Message,
                newSentMessages = messageInfo.Keys.Select((key, indx) =>
                    new SentMessages((byte)messageTypes, key, responseStatuse, DateTime.Now)
                ).ToArray()
            };

            _ = await _context.Message.AddAsync(sentMessages).ConfigureAwait(false);
            _ = await _context.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError("error in SaveSentMessagesAsync", ex);
        }
    }

    private Task SaveSentMessagesAsync(MessageInfo messageInfo, string responseStatuse, MessageTypes messageTypes)
    {
        return SaveSentMessagesAsync(messageInfo, new string[] { messageInfo.Keys.Select(_ => responseStatuse).First() }, messageTypes);
    }
    private async Task SaveSentMessagesAsync(MessageInfo messageInfo, string[] responseStatuses, MessageTypes messageTypes)
    {
        try
        {
            IEnumerable<Message> sentMessages = messageInfo.Keys.Select((key, indx) => new Message
            {
                MessageText = messageInfo.Message,
                               newSentMessages = messageInfo.Keys.Select((key, indx) =>
                      new SentMessages((byte)messageTypes, key, responseStatuses[0], DateTime.Now)
                ).ToArray()
            }).ToArray();

            await _context.Message.AddRangeAsync(sentMessages).ConfigureAwait(false);
            _ = await _context.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError("error in SaveSentMessagesAsync", ex);
                   }
    }
    public async Task<SendGrid.Response> MailSendingTender(MailInfo messageInfo, string? mailSender)
    {
        string mailfrom = string.Empty;
        try
        {
            var email = new MailAddress(mailSender);
            mailfrom = email.Address.ToLower();
        }
        catch
        {
            try
            {
                var email = new MailAddress(_configurationService.GetItemAsync(mailSender, SystemTypes.Mobile).Result.Value);
                mailfrom = email.Address.ToLower();
            }
            catch (Exception ex)
            {
                mailfrom = _configurationService.GetItemAsync("SendGridTenderMail", SystemTypes.Mobile).Result.Value;
                _logger.LogError("Sender Email invalid. Replacing with default.", ex);
                           }
        }
                      var apiKey = _apiMailSettingsConfig.Value.SendGridApiKey;
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(mailfrom, "Israel Railways");
        var subject = messageInfo.Subject;
        var plainTextContent = "Mail from Israel Railways";

        try
        {
                                  DateTime today = DateTime.Today;
            var htmlContent = messageInfo.Message;
                      

            SendGridMessage msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, messageInfo.Addresses, subject, plainTextContent, htmlContent);
                       msg.AddReplyTo("noreply@rail.co.il", "Israel Railways");

            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
                       return response;

        }
        catch (Exception ex)
        {
            _logger.LogError("error occured", ex);
            throw ex;
        }
           }
    public async Task<SendGrid.Response> MailSendingAttachment(MailInfo messageInfo, string? mailSender, SendGrid.Helpers.Mail.Attachment? fileAttachment)
    {
        string mailfrom = string.Empty;
        try
        {
            var email = new MailAddress(mailSender);
            mailfrom = email.Address.ToLower();
        }
        catch
        {
            try
            {
                var email = new MailAddress(_configurationService.GetItemAsync(mailSender, SystemTypes.Mobile).Result.Value);
                mailfrom = email.Address.ToLower();
            }
            catch (Exception ex)
            {
                mailfrom = _configurationService.GetItemAsync("SendGridTenderMail", SystemTypes.Mobile).Result.Value;
                _logger.LogError("Sender Email invalid. Replacing with default.", ex);
            }
        }
        var apiKey = _apiMailSettingsConfig.Value.SendGridApiKey;
        var client = new SendGridClient(apiKey);
        var subject = (string.IsNullOrEmpty(messageInfo.Subject) || messageInfo.Subject.Length == 0) ? "Israel Railways" : messageInfo.Subject;
        try
        {
            SendGridMessage msg = new();
            msg = MailHelper.CreateSingleEmailToMultipleRecipients(new EmailAddress(mailfrom, "Israel Railways"),
                messageInfo.Addresses, subject, subject,
                (string.IsNullOrEmpty(messageInfo.Message) || messageInfo.Message.Length == 0) ? "Mail From Israel Railways." : messageInfo.Message);
            msg.AddAttachment(fileAttachment);
            DateTimeOffset dto = new DateTimeOffset(DateTime.Now.ToUniversalTime());
            dto.ToUnixTimeSeconds();
            msg.SetSendAt(dto.ToUnixTimeSeconds());
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occured", ex);
            throw ex;
        }
    }
}
