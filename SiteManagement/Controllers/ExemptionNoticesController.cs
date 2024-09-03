using AutoMapper;
using Core.Entities;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteManagement.Dtos;


namespace SiteManagement.Controllers;

public class ExemptionNoticesController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IExemptionNotices _exemptionNotices;

    public ExemptionNoticesController(IExemptionNotices exemptionNotices, IMapper mapper)
    {
        _mapper = mapper;
        _exemptionNotices = exemptionNotices;
    }

    [HttpGet("{categoryId}")]
    [Authorize(Policy = "PageRole")]
    public async Task<IEnumerable<ExemptionNoticesDto>> GetAppExemptionNotices(int categoryId)
    {
        IEnumerable<ExemptionNoticesDto> tenders = await _exemptionNotices.GetExemptionNoticesAsync(categoryId);
        return tenders;
    }

    [HttpGet]
    [Authorize(Policy = "PageRole")]
    public async Task<IEnumerable<ExemptionNoticesDto>> GetAppExemptionNotices()
    {
        IEnumerable<ExemptionNoticesDto> tenders = await _exemptionNotices.GetExemptionNoticesNoCache();
        return tenders;
    }

                     
    [HttpPost]
   [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<Guid>> AddTenders(ExemptionNoticesPostDto request)
    {
        ActionResult<Guid> result;

        ExemptionNotices requseEnt =  _mapper.Map<ExemptionNotices>(request);

        try
        {
            ExemptionNotices responseENT = await _exemptionNotices.AddExemptionNoticesAsync(requseEnt);
            return Ok(responseENT.Id);
        }
        catch (Exception ex)
        {
           
           return BadRequest(new ApiErrorResponse { Message = "שמירה נכשלה", StatusCode = 400 });
        }
    }

    [HttpPut]
   [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<ExemptionNoticesPutDto>> UpdateTenders(ExemptionNoticesPutDto request)
    {
        ActionResult<ExemptionNoticesPutDto> result;

        ExemptionNotices requseEnt = _mapper.Map<ExemptionNotices>(request);

        try
        {
            ExemptionNotices responseENT = await _exemptionNotices.UpdateExemptionNoticesAsync(requseEnt);
            ExemptionNoticesPutDto res = _mapper.Map<ExemptionNoticesPutDto>(responseENT); 
            return Ok(res);
        }
        catch
        {
            result = BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשלה", StatusCode = 400 });
        }

        return result;
    }
    
    [HttpDelete("{id}")]
   [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> deleteTenders(Guid id)
    {
        var error = new ClientErrorData();
        ActionResult<bool> result;
        try
        {
            bool res = await _exemptionNotices.DeleteExemptionNoticesAsync(id);
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
