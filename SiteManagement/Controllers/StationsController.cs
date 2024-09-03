using AutoMapper;
using Core.Entities.Vouchers;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteManagement.Dtos;

namespace SiteManagement.Controllers;

public class StationsController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IStationsService _stationsService;

    public StationsController(IStationsService stationsService, IMapper mapper)
    {
        _mapper = mapper;
        _stationsService = stationsService;
    }

    [HttpGet("{id}")]
       public async Task<StationDto> GetStation(int id)
    {
        Station station = await _stationsService.GetStationNoCache(id);
        StationInfo stationInfo = await _stationsService.GetStationInfoAsync(id);
        StationDto stationDto = _mapper.Map<StationDto>(station);
        stationDto = _mapper.Map(stationInfo, stationDto);
        return stationDto;
    }

    [HttpGet]
       public async Task<IEnumerable<StationDto>> GetStations()
    {
        IEnumerable<Station> stations = await _stationsService.GetStationsNoCache();
        IEnumerable<StationDto> stationsDto = _mapper.Map<IEnumerable<StationDto>>(stations);
        return stationsDto;
    }

    [HttpGet("GetStationsInfo")]
       public async Task<IEnumerable<StationInfo>> GetStationsInfo()
    {
        IEnumerable<StationInfo> stationInfo = await _stationsService.GetStationsInfoAsync();
        return stationInfo;
    }


    [HttpGet("GetParkingCosts")]
       public async Task<IEnumerable<ParkingCosts>> GetParkingCosts()
    {
        IEnumerable<ParkingCosts> parkingCosts = await _stationsService.GetParkingCostsAsync();
        return parkingCosts;
    }

    [HttpPost]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<StationDto>> AddStation(StationDto request)
    {
        ActionResult<StationDto> result;

        Station station = _mapper.Map<Station>(request);

        try
        {
            Station newStation = await _stationsService.AddStationAsync(station);
            StationDto stationDto = _mapper.Map<StationDto>(newStation);
            result = Ok(stationDto);
        }
        catch (Exception ex)
        {
            var error = new ClientErrorData();

            if (ex.InnerException.Message.ToLower().Contains("duplicate key"))
            {
                error.Title = "מזהה בתפעול שאתה מנסה לשמור כבר קיים במערכת";
            }
            else
            {
                error.Title = ex.InnerException.Message;
            }
            result = BadRequest(new ApiErrorResponse { Message = error.Title, StatusCode = 400 }); 
        }

        return result;
    }

    [HttpPut]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<StationDto>> updateStation(StationUpdateDto request)
    {
        ActionResult<StationDto> result;

        Station station = _mapper.Map<Station>(request);

        try
        {
            Station newStation = await _stationsService.UpdateStationAsync(station);
            StationDto stationDto = _mapper.Map<StationDto>(newStation);
            result = Ok(stationDto);
        }
        catch (Exception ex)
        {
            var error = new ClientErrorData();
            error.Title = "failed to update a station";
            result = BadRequest(error);
        }

        return result;
    }


    [HttpPut("UpdateNonActiveElavators")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<StationInfo>> UpdateNonActiveElavators(StationElevatorsDto request)
    {
        ActionResult<StationInfo> result;

       
        try
        {
            StationInfo StationInfo = await _stationsService.UpdateNonActiveElavatorsAsync(request.nonActiveElavators, request.stationId);
                       result = Ok(StationInfo);
        }
        catch (Exception ex)
        {
            result = BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשלה", StatusCode = 400 });
        }

        return result;
    }

    [HttpDelete("{Id}")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> deleteStation(int Id)
    {
        var error = new ClientErrorData();
        ActionResult<bool> result;
        try
        {
            bool res = await _stationsService.DeleteStationAsync(Id);
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
