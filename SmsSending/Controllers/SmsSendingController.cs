using AutoMapper;
using Core.Entities.Messenger;
using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SmsSending.Dtos;

namespace SmsSending.Controllers;

public class SmsSendingController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IMessengerService _messengerService;

    public SmsSendingController(IMapper mapper, IMessengerService messengerService)
    {
        _mapper = mapper;
        _messengerService = messengerService;
    }
    [HttpPost("SendMessage")]
    public async Task<ActionResult<bool>> SendMessage(MessageInfoDto messageInfoDto)
    {
        bool result = false;
        if (messageInfoDto == null)
            return BadRequest(false);
        MessageInfo messageInfo = new()
        {
            Message = messageInfoDto.message,
            MessageType = Core.Enums.MessageTypes.SMS,
            Keys = messageInfoDto.numbers
        };

        switch (messageInfoDto.subscriber)
        {
            case SmsSubscriberType.Inforu:
                break;
            case SmsSubscriberType.Telemassage:
                break;
            case SmsSubscriberType.UniCell:
                result = await _messengerService.SendUniCellSmsAsync(messageInfo).ConfigureAwait(false);
                //await _mailService.SendEmailAsync(new Core.Entities.Mail.MailRequest { Subject = $"error in translation with key: {translation.Key}", Body = ex.Message }).ConfigureAwait(false);
                break;
            default:
                break;
        }
        return Ok(result);
    }
}
