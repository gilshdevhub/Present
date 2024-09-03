using AutoMapper;
using Core.Entities.AppMessages;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteManagement.Dtos;

namespace SiteManagement.Controllers;

public class PopUpMessagesController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IPopUpMessagesService _messagesService;

    public PopUpMessagesController(IPopUpMessagesService messagesService, IMapper mapper)
    {
        _mapper = mapper;
        _messagesService = messagesService;
    }

    [HttpGet]
    [Authorize(Policy = "PageRole")]
    public async Task<IEnumerable<PopUpMessagesResponseDto>> GetAppMessages()
    {
        IEnumerable<PopUpMessages> appMessages = await _messagesService.GetMessagesAsync();
        IEnumerable<PopUpMessagesResponseDto> messagesDto = _mapper.Map<IEnumerable<PopUpMessagesResponseDto>>(appMessages);
        return messagesDto;
    }
    [HttpGet("Station/{StationId}")]
    [Authorize(Policy = "PageRole")]
    public async Task<IEnumerable<PopUpMessagesResponseDto>> GetAppMessagesByStation(int StationId)
    {
        IEnumerable<PopUpMessages> appMessages = await _messagesService.GetMessagesByStationAsync(StationId);
        IEnumerable<PopUpMessagesResponseDto> messagesDto = _mapper.Map<IEnumerable<PopUpMessagesResponseDto>>(appMessages);
        return messagesDto;
    }
    [HttpPost]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> AddMessage(PopUpMessagesRequestDto request)
    {
        ActionResult<bool> result;

        PopUpMessages message = _mapper.Map<PopUpMessages>(request);

        try
        {
            bool res = await _messagesService.AddMessageAsync(message);

            result = Ok(res);
        }
        catch
        {
            result = BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשלה", StatusCode = 400 });
        }

        return result;
    }

    [HttpGet("pagetypes")]
    [Authorize]
    public async Task<IEnumerable<PageType>> GetPageTypes()
    {
        return await _messagesService.GetPageTypesAsync();
    }


    [HttpPut]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> UpdateMessage(PopUpMessagesRequestDto request)
    {
        ActionResult<bool> result;

        PopUpMessages message = _mapper.Map<PopUpMessages>(request);

        try
        {
            bool res = await _messagesService.UpdateMessageAsync(message);
            result = Ok(res);
        }
        catch
        {
            result = BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשלה", StatusCode = 400 });
        }

        return result;
    }
    [HttpDelete("{id}")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> deleteMessage(int id)
    {
        var error = new ClientErrorData();
        ActionResult<bool> result;
        try
        {
            bool res = await _messagesService.DeleteMessageAsync(id);
            if (res)
            {
                result = Ok(true);
            }
            else
            {
                result = BadRequest(new ApiErrorResponse { Message = "מחיקה נכשלה", StatusCode = 400 });
            }
        }
        catch
        {
            result = BadRequest(new ApiErrorResponse { Message = "מחיקה נכשלה", StatusCode = 400 });
        }

        return result;
    }

    [HttpDelete("Station/{StationId}")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> deleteMessageByStation(int StationId)
    {
        var error = new ClientErrorData();
        ActionResult<bool> result;
        try
        {
            bool res = await _messagesService.DeleteMessageByStationAsync(StationId);
            if (res)
            {
                result = Ok(true);
            }
            else
            {
                result = BadRequest(new ApiErrorResponse { Message = "מחיקה נכשלה", StatusCode = 400 });
            }
        }
        catch
        {
            result = BadRequest(new ApiErrorResponse { Message = "מחיקה נכשלה", StatusCode = 400 });
        }

        return result;
    }
}
