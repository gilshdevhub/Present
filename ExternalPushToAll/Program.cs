using Core.Entities.Messenger;
using Core.Entities.Push;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExternalPushToAll
{
    public class Program
    {
        private readonly RailDbContext _context;
        private readonly IMessengerService _messengerService;
        const int maxCount = 400;//מספר מקסימלי של פושים שאנו שולחים בבד אחד. 
                                 // Firebase can send maximum 500 messages
        public Program(RailDbContext context, IMessengerService messengerService)
        {
            _context = context;
            _messengerService = messengerService;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
        public async Task<bool> ExternalWithFirebaseAsync(MessageInfo messageInfo)
        {
            //int maxCount = 450; //מספר מקסימלי של פושים שאנו שולחים בבד אחד. 
            // Firebase can send maximum 500 messages
            bool isSuccess = false;
            int index = 0;
            List<string> tokenIds = new List<string>();
            string[] temp;
            string[] responseStatuses = { };
            List<PushRegistration> activeTokenIds = await _context.PushRegistrations.Where(p => p.State == (int)RegistrationState.Signed).ToListAsync();

            if (messageInfo.MessageType == MessageTypes.Push)
            {
                string[] tempKeys = messageInfo.Keys[0].Split(",");
                tokenIds = activeTokenIds.Select(p => p.TokenId).ToList();
                temp = activeTokenIds.Select(p => p.Id.ToString()).ToArray();
                _messengerService.getKeys(temp, messageInfo);
            }
            else if (messageInfo.MessageType == MessageTypes.PushToAll)
            {
                tokenIds = activeTokenIds.Select(p => p.TokenId).ToList();
                temp = activeTokenIds.Select(p => p.Id.ToString()).ToArray();
                _messengerService.getKeys(temp, messageInfo);
            }

            if (tokenIds.Any())
            {
                var listTokenIds = new List<List<string>>();

                for (int i = 0; i < tokenIds.Count; i += maxCount)
                {
                    listTokenIds.Add(tokenIds.GetRange(i, Math.Min(maxCount, tokenIds.Count - i)));
                }

                foreach (var tokenIdsBulk in listTokenIds)
                {
                    FirebaseAdmin.Messaging.MulticastMessage messages = new()
                    {
                        Tokens = tokenIdsBulk.ToArray(),
                        Notification = new FirebaseAdmin.Messaging.Notification { Body = messageInfo.Message, Title = messageInfo.Subject }
                    };

                    FirebaseAdmin.Messaging.BatchResponse result = await FirebaseAdmin.Messaging.FirebaseMessaging.DefaultInstance.SendMulticastAsync(messages).ConfigureAwait(false);

                    //var okCount = result.Responses.Count(p => p.IsSuccess == true);
                    //var faultCount = result.Responses.Count(p => p.IsSuccess == false);
                    responseStatuses = result.Responses.Select(p => $"Successful: {p.IsSuccess} Message Id: {p.MessageId} Exception Message: {(p.Exception != null ? p.Exception.Message : null)}").ToArray();
                    //_logger.LogInformation("{0}", result.Responses.Select(p => $"Is Success: {p.IsSuccess} Message Id: {p.MessageId} Exception Message: {(p.Exception != null ? p.Exception.Message : null)}").ToArray());

                    // await SaveSentMessagesAsync(messageInfo, responseStatuses[0], MessageTypes.Push).ConfigureAwait(false);
                    isSuccess = result.SuccessCount > 0;
                    index++;
                }
                //SaveSentMessagesNewAsync
                await _messengerService.sa(messageInfo, responseStatuses[0], MessageTypes.Push).ConfigureAwait(false);
            }

            return isSuccess;
        }
    }
}