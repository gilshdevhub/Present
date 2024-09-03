using AutoMapper;
using Core.Entities.Messenger;
using Core.Entities.Push;
using Core.Enums;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteManagement.Dtos;

namespace SiteManagement.Controllers;

public class PushNotificationController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly ISpecialPushNotificationsService _specialPushNotificationsService;
    private readonly IPushNotificationsService _pushNotificationsService;
    private readonly IMessengerService _messengerService;
    private readonly IManagmentSystemObjects _managmentSystemObjects;

    public PushNotificationController(ISpecialPushNotificationsService specialPushNotificationsService,
        IMessengerService messengerService,
        IMapper mapper,
        IPushNotificationsService pushNotificationsService,
        IManagmentSystemObjects managmentSystemObjects)
    {
        _mapper = mapper;
        _messengerService = messengerService;
        _specialPushNotificationsService = specialPushNotificationsService;
        _pushNotificationsService = pushNotificationsService;
        _managmentSystemObjects = managmentSystemObjects;
    }

    [HttpGet]
    [Authorize(Policy = "PageRole")]
    public async Task<IEnumerable<PushNotificationAndRegistrationIds>> GetPushRegistrationIDs([FromQuery] FilterParametrs parametrs)
    {
        IEnumerable<PushNotificationAndRegistrationIds> appMessages = await _specialPushNotificationsService.GetPushRegistrationIDs(parametrs);
        return appMessages;
    }

    [HttpGet("getPushRegistrationIDsMaslul")]
    [Authorize(Policy = "PageRole")]
    public async Task<IEnumerable<PushNotificationAndRegistrationIds>> getPushRegistrationIDsMaslul([FromQuery] FilterParametrsMaslul parametrs)
    {
        IEnumerable<PushNotificationAndRegistrationIds> appMessages = await _specialPushNotificationsService.GetPushRegistrationIDsMaslul(parametrs);
        return appMessages;
    }


                           
    [Authorize(Policy = "PageRole")]
    [HttpGet("log")]
    public async Task<ActionResult<IEnumerable<PushLogResponseDto>>> GetPushLog([FromQuery] PushLogRequestDto pushLogRequestDto)
    {
        PushLogRequest pushLogRequest = _mapper.Map<PushLogRequest>(pushLogRequestDto);
        IEnumerable<PushNotificationsLog> pushNotificationsLogs = await _specialPushNotificationsService.GetPushLogAsync(pushLogRequest).ConfigureAwait(false);
        IEnumerable<PushLogResponseDto> pushLogResponseDtos = _mapper.Map<IEnumerable<PushLogResponseDto>>(pushNotificationsLogs);
        return Ok(pushLogResponseDtos);
    }

    [Authorize(Policy = "PageRole")]
    [HttpGet("sentmessages")]
    public async Task<ActionResult<IEnumerable<SentMessageResponse>>> GetSentMessages([FromQuery] SentMessageCriteriaDto request)
    {
        SentMessageCriteria sentMessageRequest = _mapper.Map<SentMessageCriteria>(request);

        IEnumerable<SentMessageResponse> response = await _messengerService.GetSentMessagesAsync(sentMessageRequest).ConfigureAwait(false);
        return Ok(response);
    }

    [Authorize(Policy = "PageRole")]
    [HttpPost("sendmessage")]
    public async Task<ActionResult<bool>> SendMessage(MessageInfoDto messageInfoDto)
    {
        bool result = false;
        MessageInfo messageInfo = _mapper.Map<MessageInfo>(messageInfoDto);

        switch (messageInfoDto.MessageType)
        {
            case MessageTypes.Push:
            case MessageTypes.PushToAll:
                result = await _messengerService.SendPushAsync(messageInfo).ConfigureAwait(false);
                break;
            case MessageTypes.SMS:
            case MessageTypes.Mail:
                result = await _messengerService.SendUniCellSmsAsync(messageInfo).ConfigureAwait(false);
                break;
        }

        return Ok(result);
    }

    [Authorize(Policy = "PageRole")]
    [HttpGet("getTrains")]
    public async Task<ActionResult<IEnumerable<TrainsDto>>> GetTrains()
    {
        IEnumerable<int> trains = await _messengerService.GetTrains();
        IEnumerable<TrainsDto> result = _mapper.Map<IEnumerable<TrainsDto>>(trains);


        return Ok(result);
    }

    [Authorize(Policy = "PageRole")]
    [HttpGet("GetStationsIDs")]
    public async Task<ActionResult<IEnumerable<MaslulResponse>>> GetStationsIDs([FromQuery] StationsRequest stationsRequest)
    {
        IEnumerable<MaslulResponse> stations = await _messengerService.GetStationsIDs(stationsRequest);

        return Ok(stations);
    }
    [Authorize(Policy = "PageRole")]
    [HttpPost("GetPushCount")]
    public async Task<ActionResult<SendFirebaseMessagesAsyncDto>> GetPushCount(MessageInfoDto messageInfoDto)
    {
        MessageInfo messageInfo = _mapper.Map<MessageInfo>(messageInfoDto);
        string buttonState = await _messengerService.CheckButtomState(MessageTypes.Push);
        string buttonState2 = await _messengerService.CheckButtomState(MessageTypes.PushToAll);
        if (buttonState != "true" || buttonState2 != "true")
        {
            return UnprocessableEntity(new ApiErrorResponse { Message = "הפעולה בוטלה", StatusCode = 422 });
        }

        SendFirebaseMessagesAsyncDto sendFirebaseMessagesAsyncDto = await _messengerService.CountPushWithFirebaseAsync(messageInfo);
        return Ok(sendFirebaseMessagesAsyncDto);
    }

    [Authorize(Policy = "PageRole")]
    [HttpPost("GetPushCountToAll")]
    public async Task<ActionResult<SendFirebaseMessagesAsyncDto>> GetPushCountToAll()
    {
        string buttonState = await _messengerService.CheckButtomState(MessageTypes.Push);
        string buttonState2 = await _messengerService.CheckButtomState(MessageTypes.PushToAll);
        if (buttonState != "true" || buttonState2 != "true")
        {
            return UnprocessableEntity(new ApiErrorResponse { Message = "הפעולה בוטלה", StatusCode = 422 });
        }

        int res = await _messengerService.CountPushToAllAsync();
        return Ok(res);
    }

    [Authorize(Policy = "PageRole")]
    [HttpPost("SendFirebaseMessagesAsync")]
    public async Task<ActionResult<bool>> SendFirebaseMessagesAsync(SendFirebaseMessagesAsyncDto sendFirebaseMessagesAsyncDto)
    {
        try
        {
            return Ok(true);
        }
        finally
        {
            Response.OnCompleted(async () =>
            {
                await _messengerService.SendFirebaseMessagesAsync(sendFirebaseMessagesAsyncDto);// tokenIds, messageInfo);
            });
        }
    }


    [Authorize(Policy = "PageRole")]
    [HttpPost("SendPushToallAsync")]
    public async Task<ActionResult<bool>> SendPushToallAsync(MessageTextDTO messageTextDto)
    {
        try
        {
            return Ok(true);
        }
        finally
        {
            Response.OnCompleted(async () =>
            {
                await _messengerService.SendPushToAllMessagesAsync(messageTextDto.messageText);
            });
        }
    }


    [Authorize(Policy = "PageRole")]
    [HttpPost("CheckButtomState")]
    public async Task<ActionResult<string>> CheckButtomState(GetPushCountDto getPushCountDtoe)
    {
        string buttonState = await _messengerService.CheckButtomState(MessageTypes.Push);
        string buttonState2 = await _messengerService.CheckButtomState(MessageTypes.PushToAll);
        if (buttonState != "true")
        {
                       return UnprocessableEntity(new ApiErrorResponse { Message = "הפעולה בוטלה", StatusCode = 422 });
        }
        if (buttonState2 != "true")
        {
            return UnprocessableEntity(new ApiErrorResponse { Message = "הפעולה בוטלה", StatusCode = 422 });
        }

        return Ok("true");
    }
}
