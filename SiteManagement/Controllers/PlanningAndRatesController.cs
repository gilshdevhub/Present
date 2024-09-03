using AutoMapper;
using Core.Entities;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace SiteManagement.Controllers;

public class PlanningAndRatesController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IPlanningAndRatesService _planningAndRates;

    public PlanningAndRatesController(IPlanningAndRatesService planningAndRates, IMapper mapper)
    {
        _mapper = mapper;
        _planningAndRates = planningAndRates;
    }

    [HttpGet("{categoryId}")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<IEnumerable<PlanningAndRatesDto>>> GetAppPlanningAndRates(int categoryId)
    {
        IEnumerable<PlanningAndRatesDto> tenders = await _planningAndRates.GetPlanningAndRatesAsync(categoryId);
        return Ok(tenders);
    }

    [HttpGet]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<IEnumerable<PlanningAndRatesDto>>> GetAppPlanningAndRates()
    {
        try
        {
            IEnumerable<PlanningAndRatesDto> tenders = await _planningAndRates.GetNoCache();
            return Ok(tenders);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

                     
    [HttpPost]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<Guid>> AddPlanningAndRates(PlanningAndRatesDto request)
    {
        ActionResult<Guid> result;

        PlanningAndRates requseEnt =  _mapper.Map<PlanningAndRates>(request);
        requseEnt.Id = Guid.Empty;
        try
        {
            PlanningAndRates responseENT = await _planningAndRates.AddPlanningAndRatesAsync(requseEnt);
            result = Ok(responseENT.Id);
        }
        catch (Exception ex)
        {
            result = BadRequest(new ApiErrorResponse { Message = "שמירה נכשלה", StatusCode = 400 });
        }

        return result;
    }

    [HttpPut]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<PlanningAndRates>> UpdatePlanningAndRates(PlanningAndRatesDto request)
    {
        ActionResult<PlanningAndRates> result;

        PlanningAndRates requseEnt = _mapper.Map<PlanningAndRates>(request);

        try
        {
            PlanningAndRates responseENT = await _planningAndRates.UpdatePlanningAndRatesAsync(requseEnt);
            result = Ok(responseENT);
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
            bool res = await _planningAndRates.DeletePlanningAndRatesAsync(id);
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
