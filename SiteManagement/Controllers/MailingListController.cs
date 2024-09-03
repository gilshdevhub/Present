using AutoMapper;
using Core.Entities;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteManagement.Dtos;
using System.Text.Json;

namespace SiteManagement.Controllers;

public class MailingListController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IMailingListService _mailingListService;

    public MailingListController(IMailingListService mailingListService, IMapper mapper)
    {
        _mapper = mapper;
        _mailingListService = mailingListService;
    }

    [HttpGet]
    [Authorize(Policy = "PageRole")]
    public async Task<IEnumerable<MailingList>> GetMailingLists()
    {
        IEnumerable<MailingList> mailingLists = await _mailingListService.GetMailingListsContentNoCache();
        return mailingLists;
    }
    [HttpGet("{id}")]
    [Authorize(Policy = "PageRole")]
    public async Task<MailingList> GetMailingListById(int id)
    {
        MailingList mailingList = await _mailingListService.GetMailingListByIdNoCache(id);
        return mailingList;
    }

    [HttpPost]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<MailingList>> AddMailingList(MailingListPostDto request)
    {
        ActionResult<MailingList> result;

        MailingList requseEnt = _mapper.Map<MailingList>(request);

        try
        {
            MailingList responseENT = await _mailingListService.AddMailingListAsync(requseEnt);
            MailingListPostDto res = JsonSerializer.Deserialize<MailingListPostDto>(JsonSerializer.Serialize(responseENT));
            result = Ok(responseENT);
        }
        catch(Exception ex)
        {
            result = BadRequest(new ApiErrorResponse { Message = "שמירה נכשלה", StatusCode = 400 });
        }

        return result;
    }
    [HttpPut("RemoveSingleMailFromList")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<MailingList>> RemoveSingleMailFromList(string mail)
    {
        bool result = await _mailingListService.DeleteSingleMailAsync(mail).ConfigureAwait(false);
        if (!result)
            return BadRequest(new ApiErrorResponse { Message = "מחיקה נכשלה", StatusCode = 400 });

        return Ok(result);
    }

    [HttpPut]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<MailingListPutDto>> UpdateMailingList(MailingListPutDto request)
    {
        ActionResult<MailingListPutDto> result;

        MailingList requseEnt = _mapper.Map<MailingList>(request);

        try
        {
            MailingList responseENT = await _mailingListService.UpdateMailingListAsync(requseEnt);
            MailingListPutDto res = JsonSerializer.Deserialize<MailingListPutDto>(JsonSerializer.Serialize(responseENT));
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
    public async Task<ActionResult<bool>> DeleteMailingList(int id)
    {
        var error = new ClientErrorData();
        ActionResult<bool> result;
        try
        {
            bool res = await _mailingListService.DeleteMailingListAsync(id);
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
