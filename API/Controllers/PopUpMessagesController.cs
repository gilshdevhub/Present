using API.Dtos.PopupMessages;
using AutoMapper;
using Core.Entities.AppMessages;
using Core.Filters;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ServiceFilter(typeof(WriteToLogFilterAttribute))]
public class PopUpMessagesController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IPopUpMessagesService _popupMessageService;

    public PopUpMessagesController(IMapper mapper, IPopUpMessagesService popUpMessagesService)
    {
        _mapper = mapper;
        _popupMessageService = popUpMessagesService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PopupMessagesResponseDto>>> GetPopUpMessages([FromQuery] PopupMessagesRequestDto popupMessagesRequestDto)
    {
        MessageRequest messageRequest = _mapper.Map<MessageRequest>(popupMessagesRequestDto);
        IEnumerable<PopUpMessages> messages = (await _popupMessageService.GetMessagesAsync(messageRequest).ConfigureAwait(false)).Where(x=>string.IsNullOrEmpty(x.StationsIds));
        IEnumerable<PopupMessagesResponseDto> popupMessagesResponseDtos = _mapper.Map<IEnumerable<PopupMessagesResponseDto>>(messages, opt => opt.Items["languageId"] = popupMessagesRequestDto.LanguageId);
        return Ok(popupMessagesResponseDtos);
    }
    [HttpGet("GetPopUpMessagesWithStation")]
    public async Task<ActionResult<IEnumerable<PopupMessagesWithStationsResponseDto>>> GetPopUpMessagesWithStation([FromQuery] PopupMessagesStationRequestDto popupMessagesRequestDto)
    {
        MessageRequest messageRequest = _mapper.Map<MessageRequest>(popupMessagesRequestDto);
        IEnumerable<PopUpMessages> messages = (await _popupMessageService.GetMessagesAsync(messageRequest).ConfigureAwait(false)).Where(x => !string.IsNullOrEmpty(x.StationsIds));
        IEnumerable<PopupMessagesWithStationsResponseDto> popupMessagesResponseDtos = _mapper.Map<IEnumerable<PopupMessagesWithStationsResponseDto>>(messages, opt => opt.Items["languageId"] = popupMessagesRequestDto.LanguageId);
        return Ok(popupMessagesResponseDtos);
    }
}
