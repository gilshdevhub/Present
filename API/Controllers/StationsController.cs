using API.Dtos;
using AutoMapper;
using Core.Entities.Vouchers;
using Core.Errors;
using Core.Filters;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[ServiceFilter(typeof(WriteToLogFilterAttribute))]
public class StationsController : BaseApiController
{
    private readonly IStationsService _stationsService;
    private readonly IMapper _mapper;

    public StationsController(IStationsService stationsService, IMapper mapper)
    {
        _stationsService = stationsService;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StationResponseDto>> GetStationAsync(int id, [FromQuery] StationRequestDto request)
    {
        Station station = await _stationsService.GetStationAsync(id).ConfigureAwait(false);
        StationResponseDto stationDto = _mapper.Map<StationResponseDto>(station, opt => opt.Items["languageId"] = request.LanguageId);
        return stationDto == null ? NotFound(new ApiErrorResponse((int)HttpStatusCode.NotFound, $"station {id} was not found")) : Ok(stationDto);
    }

    [HttpGet("GetStationWithInfo/{id}")]
    public async Task<ActionResult<StationResponseDto>> GetStationWithInfoAsync(int id, [FromQuery] StationRequestDto request)
    {
        Station station = await _stationsService.GetStationAsync(id).ConfigureAwait(false);
        StationResponseDto stationDto = _mapper.Map<StationResponseDto>(station, opt => opt.Items["languageId"] = request.LanguageId);
        return stationDto == null ? NotFound(new ApiErrorResponse((int)HttpStatusCode.NotFound, $"station {id} was not found")) : Ok(stationDto);
    }


    [HttpGet()]
    public async Task<ActionResult<IEnumerable<StationResponseDto>>> GetStationsAsync([FromQuery] StationRequestDto request)
    {
        IEnumerable<Station> stations = await _stationsService.GetStationsAsync().ConfigureAwait(false);
        IEnumerable<StationResponseDto> stationDtos = _mapper.Map<IEnumerable<StationResponseDto>>(stations, opt => opt.Items["languageId"] = request.LanguageId);
        return Ok(stationDtos);
    }


    [HttpGet("GetStationInformation")]
    public async Task<ActionResult<StationInformationRsponseDto>> GetStationInformation([FromQuery] StationInformationRequestDto request)
    {
        try
        {
            StationInformationRsponseDto result = new StationInformationRsponseDto();
            result = await _stationsService.GetStationInformationAsync(request).ConfigureAwait(false);
            return Ok(result);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }


    }

    [HttpGet("GetStationServices")]
    public async Task<ActionResult<List<GateServices>>> GetStationServices([FromQuery] StationInformationRequestDto request)
    {


        try
        {
            List<GateServices> result = await _stationsService.StationGateService(request).ConfigureAwait(false);
            return Ok(result);
        }
        catch (Exception ex)
        {
            throw;
        }


    }
}
