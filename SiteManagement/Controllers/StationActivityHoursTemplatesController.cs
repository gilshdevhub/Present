using AutoMapper;
using Core.Entities.Vouchers;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SiteManagement.Controllers;

public class StationActivityHoursTemplatesController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IStationActivityHoursTemplatesService _stationActivityHoursTemplatesService;

    public StationActivityHoursTemplatesController(IStationActivityHoursTemplatesService stationActivityHoursTemplatesService, IMapper mapper)
    {
        _mapper = mapper;
        _stationActivityHoursTemplatesService = stationActivityHoursTemplatesService;
    }

    [HttpGet("GetStationActivityTemplates")]
    [Authorize]
    public async Task<IEnumerable<StationActivityHoursTemplateLinesDto>> GetStationActivityTemplates()
    {
        IEnumerable<StationActivityHoursTemplateLinesDto> stationActivityHoursTemplates = await _stationActivityHoursTemplatesService.GetStationActivityHoursTemplateLinesAsync();
        return stationActivityHoursTemplates;
    }


    [HttpGet("GetStationActivityTemplates/{id}")]
    [Authorize]
    public async Task<IEnumerable<StationActivityHoursTemplateLinesDto>> GetStationActivityTemplates(int id)
    {
        IEnumerable<StationActivityHoursTemplateLinesDto> stationActivityHoursTemplates = await _stationActivityHoursTemplatesService.GetStationActivityHoursTemplateLinesAsync();
        return stationActivityHoursTemplates.Where(p => p.TemplateId == id).Select(x => x);
    }

    [HttpGet("GetStationActivityTemplatesByType")]
    [Authorize]
    public async Task<IEnumerable<StationActivityHoursTemplateLinesDto>> GetStationActivityTemplatesByType(int templateTypeId)
    {
        IEnumerable<StationActivityHoursTemplateLinesDto> stationActivityHoursTemplates = await _stationActivityHoursTemplatesService.GetStationActivityHoursTemplateLinesAsync();
        return stationActivityHoursTemplates.Where(p => p.TemplateTypeId == templateTypeId).Select(x => x);
    }

    [HttpGet("GetStationActivityTemplatesTypes")]
    [Authorize(Policy = "PageRole")]
    public async Task<IEnumerable<StationActivityTemplatesTypes>> GetStationActivityTemplatesTypes()
    {
        IEnumerable<StationActivityTemplatesTypes> stationActivityTemplatesTypes = await _stationActivityHoursTemplatesService.GetStationActivityTemplatesTypesAsync();
        return stationActivityTemplatesTypes;
    }

    [HttpGet("GetTamplatesByType")]
    [Authorize(Policy = "PageRole")]
    public async Task<IEnumerable<TemplatesDto>> GetTamplatesByType([FromQuery] int templateType)
    {
        IEnumerable<TemplatesDto> templatesDto = await _stationActivityHoursTemplatesService.GetTamplatesByType(templateType);
        return templatesDto;
    }


    [HttpPost]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<StationActivityHoursTemplates>> InsertTemplate(StationActivityHoursTemplatesPostDto request)
    {
        ActionResult<StationActivityHoursTemplates> result;
        StationActivityHoursTemplates stationActivityHoursTemplates = _mapper.Map<StationActivityHoursTemplates>(request.template);
        IEnumerable<StationActivityHoursTemplatesLine> stationActivityHoursTemplatesLines = _mapper.Map<IEnumerable<StationActivityHoursTemplatesLinePostDto>, IEnumerable<StationActivityHoursTemplatesLine>>(request.StationActivityHoursTemplatesLines);
        try
        {
            var res = await _stationActivityHoursTemplatesService.AddTemplateAsync(stationActivityHoursTemplates, stationActivityHoursTemplatesLines);
            result = Ok(res);
        }
        catch (Exception ex)
        {
            result = BadRequest(new ApiErrorResponse { Message = "שמירה נכשלה", StatusCode = 400 });
        }

        return result;
    }

    [HttpPut]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> UpdateTemplate(StationActivityHoursTemplatesUpdateDto request)
    {
        ActionResult<bool> result;
        StationActivityHoursTemplates stationActivityHoursTemplates = _mapper.Map<StationActivityHoursTemplates>(request.template);
        IEnumerable<StationActivityHoursTemplatesLine> stationActivityHoursTemplatesLines = _mapper.Map<IEnumerable<StationActivityHoursTemplatesLineDto>, IEnumerable<StationActivityHoursTemplatesLine>>(request.StationActivityHoursTemplatesLines);
        try
        {
            var res = await _stationActivityHoursTemplatesService.UpdateTemplateAsync(stationActivityHoursTemplates, stationActivityHoursTemplatesLines, request.DeletedIds);
            result = Ok(res);
        }
        catch (Exception ex)
        {
            result = BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשלה", StatusCode = 400 });
        }

        return result;
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> DeleteTemplate(int id)
    {
        var error = new ClientErrorData();
        ActionResult<bool> result;
        try
        {
            bool res = await _stationActivityHoursTemplatesService.DeleteTemplateAsync(id);
            if (res)
            {
                result = Ok(res);
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
