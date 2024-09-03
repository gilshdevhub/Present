using AutoMapper;
using Core.Entities.Vouchers;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SiteManagement.Controllers;

public class StationGateActivityHoursController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IStationGateActivityHoursService _stationGateActivityHourService;
    private readonly IStationGateService _stationGateService;
    private readonly IStationsService _stationsService;

    public StationGateActivityHoursController(IStationsService stationsService,
                                                IStationGateService stationGateService,
                                                IStationGateActivityHoursService stationGateActivityHourService,
                                                IMapper mapper)
    {
        _mapper = mapper;
        _stationGateActivityHourService = stationGateActivityHourService;
        _stationGateService = stationGateService;
        _stationsService = stationsService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IEnumerable<StationGateActivityHoursDto>> GetStationGateActivityHours()
    {
        IEnumerable<StationGateActivityHoursDto> StationGateActivityHoursDto = await _stationGateActivityHourService.GetStationGateActivityHoursAsync();
        return StationGateActivityHoursDto;
    }

    [HttpGet("GetHoursByStationAndTemplateType")]
    [Authorize]
    public async Task<StationGateActivityHoursByGateDto> GetHoursByStationAndTemplateType(int stationId, int templateTypeId)
    {
        StationGateActivityHoursByGateDto result = new StationGateActivityHoursByGateDto();
        IEnumerable<StationGateDto> stationGates = await _stationGateService.GetStationGateAsync();
        result.StationGateActivityHours = await _stationGateActivityHourService.GetHoursByStationAndTemplateTypeAsync(stationId, templateTypeId, stationGates);
        IEnumerable<StationInfo> stationInfo = await _stationsService.GetStationsInfoAsync();
        result.NonActiveElavators = stationInfo.Where(y => y.StationId == stationId).Select(x => x.NonActiveElavators).FirstOrDefault();
        return result;
    }

                     
    [HttpGet("ByGateIdAndTemplateId")]
    [Authorize]
    public async Task<IEnumerable<StationGateActivityHoursDto>> GetGatesByGateIdAndTemplateId([FromQuery] int templateTypeId, int stationGateId)
    {
        IEnumerable<StationGateActivityHoursDto> stationGate = await _stationGateActivityHourService.GetStationGateActivityHoursByGateIdTemplateIdAsync(templateTypeId, stationGateId);
        return stationGate;
    }



                        
    [HttpPost]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> AddStationGateActivityHour(StationGateActivityHoursPostDto request)
    {
        ActionResult<bool> result;
        StationGateActivityHours stationActivityHoursTemplates = _mapper.Map<StationGateActivityHours>(request.Hours);
        IEnumerable<StationGateActivityHoursLines> stationActivityHoursTemplatesLines =
            _mapper.Map<IEnumerable<StationGateActivityHoursLinePostDto>, IEnumerable<StationGateActivityHoursLines>>(request.StationGateActivityHoursLineDto);
        try
        {
            var res = await _stationGateActivityHourService.AddStationGateActivityHourAsync(stationActivityHoursTemplates, stationActivityHoursTemplatesLines);
            result = Ok(res);
        }
        catch (Exception ex)
        {
            result = BadRequest(new ApiErrorResponse { Message = "שמירה נכשלה", StatusCode = 400 });
        }

        return result;
    }
    [HttpPost("PostList")]
    [Authorize]
    public async Task<ActionResult<bool>> AddStationGatesActivityHour(List<StationGateActivityHoursPostDto> request)
    {
        ActionResult<bool> result;
        foreach (var item in request)
        {
            StationGateActivityHours stationActivityHoursTemplates = _mapper.Map<StationGateActivityHours>(item.Hours);
            IEnumerable<StationGateActivityHoursLines> stationActivityHoursTemplatesLines =
                _mapper.Map<IEnumerable<StationGateActivityHoursLinePostDto>, IEnumerable<StationGateActivityHoursLines>>(item.StationGateActivityHoursLineDto);
            int HoursIdByGateId = await _stationGateActivityHourService.StationHoursIdByGateId(item.Hours.StationGateId, item.Hours.TemplateTypeId);
            if (HoursIdByGateId > 0) await _stationGateActivityHourService.DeleteGateActivityHourAsync(HoursIdByGateId);
            try
            {
                var res = await _stationGateActivityHourService.AddStationGateActivityHourAsync(stationActivityHoursTemplates, stationActivityHoursTemplatesLines);
                result = Ok(res);
            }
            catch (Exception ex)
            {
                result = BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשלה", StatusCode = 400 });
            }
        }



        return true;
    }
    [HttpPut]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> UpdateStationGateActivityHour(StationGateActivityHoursPutDto request)
    {
        ActionResult<bool> result;
        StationGateActivityHours stationActivityHoursTemplates = _mapper.Map<StationGateActivityHours>(request.Hours);
        IEnumerable<StationGateActivityHoursLines> stationActivityHoursTemplatesLines =
          _mapper.Map<IEnumerable<StationGateActivityHoursLineDto>, IEnumerable<StationGateActivityHoursLines>>(request.StationGateActivityHoursLineDto);
        try
        {
            var res = await _stationGateActivityHourService.UpdateStationGateActivityHourAsync(stationActivityHoursTemplates, stationActivityHoursTemplatesLines);
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
    public async Task<ActionResult<bool>> DeleteGateActivityHour(int id)
    {
        var error = new ClientErrorData();
        ActionResult<bool> result;
        try
        {
            bool res = await _stationGateActivityHourService.DeleteGateActivityHourAsync(id);
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
