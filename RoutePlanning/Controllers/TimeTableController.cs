using AutoMapper;
using Core.Entities.TimeTable;
using Core.Filters;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RoutePlanning.Dtos;

namespace RoutePlanning.Controllers;

[ServiceFilter(typeof(WriteToLogFilterAttribute))]
public class TimeTableController : BaseApiController
{
    private readonly ITimeTableService _timeTableService;
    private readonly IMapper _mapper;

    public TimeTableController(ITimeTableService timeTableService, IMapper mapper)
    {
        _timeTableService = timeTableService;
        _mapper = mapper;
    }

    [HttpGet("searchTrainLuzForDateTime")]
    public async Task<ActionResult<TrainTimeTableRespnse>> GetTrainTimeTable([FromQuery] RoutePlanningRequest request)
    {
        TrainTimeTableRequest trainTimeTableRequest = _mapper.Map<TrainTimeTableRequest>(request);
        var timeTable = await _timeTableService.GetTrainTimeTableAsync(trainTimeTableRequest).ConfigureAwait(false);
        return Ok(timeTable);
    }

    [HttpGet("searchTrainLuzBeforeDate")]
    public async Task<ActionResult<TrainTimeTableRespnse>> GetTrainTimeTableBeforeDate([FromQuery] RoutePlanningRequest request)
    {
        TrainTimeTableRequest trainTimeTableRequest = _mapper.Map<TrainTimeTableRequest>(request);
        var timeTable = await _timeTableService.GetTrainTimeTableBeforeDateAsync(trainTimeTableRequest).ConfigureAwait(false);
        return Ok(timeTable);
    }

    [HttpGet("searchTrainLuzAfterDate")]
    public async Task<ActionResult<TrainTimeTableRespnse>> GetTrainTimeTableAfterDate([FromQuery] RoutePlanningRequest request)
    {
        TrainTimeTableRequest trainTimeTableRequest = _mapper.Map<TrainTimeTableRequest>(request);
        var timeTable = await _timeTableService.GetTrainTimeTableAfterDateAsync(trainTimeTableRequest).ConfigureAwait(false);
        return Ok(timeTable);
    }

    [HttpGet("searchTrainLuzByTrainNumber")]
    public async Task<ActionResult<TrainTimeTableRespnse>> GetTrainTimeTableByTrainNumber([FromQuery] RoutePlanningByTrainNumberRequest request)
    {
        TrainTimeTableByTrainNumberRequest trainTimeTableRequest = _mapper.Map<TrainTimeTableByTrainNumberRequest>(request);
        var timeTable = await _timeTableService.GetTrainTimeTableByTrainNumberAsync(trainTimeTableRequest).ConfigureAwait(false);
        return Ok(timeTable);
    }
}
