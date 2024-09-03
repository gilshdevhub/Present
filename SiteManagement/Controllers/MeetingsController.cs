using AutoMapper;
using Core.Entities;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteManagement.Dtos;

namespace SiteManagement.Controllers;

public class MeetingsController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IMeetingsService _meetingsService;

    public MeetingsController(IMeetingsService meetingsService, IMapper mapper)
    {
        _mapper = mapper;
        _meetingsService = meetingsService;
    }

    [HttpGet]
    [Authorize(Policy = "PageRole")]
    public async Task<IEnumerable<MeetingsDto>> GetAppMeetings()
    {
        IEnumerable<MeetingsDto> meetings = await _meetingsService.GetMeetingsNoCache();
        return meetings;
    }
    [HttpGet("{id}")]
    [Authorize(Policy = "PageRole")]
    public async Task<MeetingsDto> GetAppMeetingById(int id)
    {
        MeetingsDto meeting = await _meetingsService.GetMeetingByIdAsync(id);
        return meeting;
    }

    [HttpPost]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<int>> AddMeetings(MeetingsPutDto request)
    {
        ActionResult<int> result;

        Meetings meeting = _mapper.Map<Meetings>(request);

        try
        {
            Meetings meetings = await _meetingsService.AddMeetingsAsync(meeting);

            result = Ok(meetings.MeetingsId);
        }
        catch
        {
            result = BadRequest(new ApiErrorResponse { Message = "שמירה נכשלה", StatusCode = 400 });
        }

        return result;
    }

    [HttpPut]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> UpdateMeetings(MeetingsPutDto request)
    {
        ActionResult<bool> result;

        Meetings requseEnt = _mapper.Map<Meetings>(request);

        try
        {
            bool res = await _meetingsService.UpdateMeetingsAsync(requseEnt);
                   result = Ok(res);
        }
        catch (Exception ex)
        {
            result = BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשלה", StatusCode = 400 });
        }

        return result;
    }
    [HttpDelete("{id}")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> deleteMeetings(int id)
    {
        var error = new ClientErrorData();
        ActionResult<bool> result;
        try
        {
            bool res = await _meetingsService.DeleteMeetingsAsync(id);
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
