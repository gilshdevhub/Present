using AutoMapper;
using Core.Entities.Vouchers;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteManagement.Dtos;

namespace SiteManagement.Controllers;

public class StationGateController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IStationGateService _stationGateService;

    public StationGateController(IStationGateService stationGateService, IMapper mapper)
    {
        _mapper = mapper;
        _stationGateService = stationGateService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IEnumerable<StationGateDto>> GetStationsGates()
    {
        IEnumerable<StationGateDto> stationGate = await _stationGateService.GetStationGateAsync();
        return stationGate;
    }
    [HttpGet("GetStationsGates/{id}")]
    [Authorize]
    public async Task<IEnumerable<StationGateDto>> GetStationsGates(int id)
    {
               IEnumerable<StationGateDto> stationGate = await _stationGateService.GetStationGateAsync();
        return stationGate.Where(p => p.StationGateId == id).Select(x => x);
    }

    [HttpGet("GetGateByStationId/{id}")]
    [Authorize]
    public async Task<IEnumerable<StationGateDto>> GetGateByStationId(int id)
    {
               IEnumerable<StationGateDto> stationGate = await _stationGateService.GetStationGateAsync();
        return stationGate.Where(p => p.StationId == id).Select(x => x);
    }

    [HttpGet("GetAllStationIdServices/{id}")]
    [Authorize]
    public async Task<ActionResult> GetAllStationIdServices(int id)
    {
               var stationGate = (await _stationGateService.GetStationGateAsync().ConfigureAwait(false)).AsQueryable()
            .Where(p => p.StationId == id)
            .SelectMany(x => x.StationGateServices.Distinct());
        var f = stationGate.GroupBy(x => x.ServiceId).Select(y => y.FirstOrDefault()).ToList();//.GroupBy(x => x.StationServices);


        return Ok(f);
    }


    [HttpGet("GetStationServices")]
    [Authorize]
    public async Task<IEnumerable<StationServices>> GetStationServices()
    {
        IEnumerable<StationServices> stationGate = await _stationGateService.GetStationServicesAsync();
        return stationGate;
    }

    [HttpPost]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> InsertStationGate(StationGateRequestInsertDto request)
    {
        ActionResult<bool> result;
        StationGate stationGate = _mapper.Map<StationGate>(request);
        try
        {
            var res = await _stationGateService.InsertStationGateAsync(stationGate, request.StationInfoTranslation);
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
    public async Task<ActionResult<bool>> UpdateStationGate(StationGateRequestDto request)
    {
        ActionResult<bool> result;
        StationGate stationGate = _mapper.Map<StationGate>(request);
        try
        {
            var res = await _stationGateService.UpdateStationGateAsync(stationGate, request.StationInfoTranslation);
            result = Ok(res);
        }
        catch (Exception ex)
        {
            result = BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשלה", StatusCode = 400 });
        }

        return result;
    }

    [HttpDelete("{Id}")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<IEnumerable<StationDto>>> deleteStationGate(int Id)
    {
        var error = new ClientErrorData();
        ActionResult<IEnumerable<StationDto>> result;
        try
        {
            IEnumerable<Station> stations = await _stationGateService.DeleteStationGateAsync(Id);

            result = Ok(_mapper.Map<IEnumerable<StationDto>>(stations));
                      
                                                                                     }
        catch
        {
            result = BadRequest(new ApiErrorResponse { Message = "מחיקה נכשלה", StatusCode = 400 });
        }

        return result;
    }
}
