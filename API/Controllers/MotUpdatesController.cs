﻿using API.Dtos;
using AutoMapper;
using Core.Entities.MotUpdates;
using Core.Errors;
using Core.Filters;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace API.Controllers;
[ServiceFilter(typeof(WriteToLogFilterAttribute))]
public class MotUpdatesController : BaseApiController
{
    private readonly IMotUpdatesService _motUpdatesService;
    private readonly IMapper _mapper;

    public MotUpdatesController(IMotUpdatesService motUpdatesService, IMapper mapper)
    {
        _motUpdatesService = motUpdatesService;
        _mapper = mapper;
    }

    [HttpGet("GetMotUpdates")]
    public async Task<ActionResult<MotUpdateResponse>> GetMotUpdates([FromQuery] MotUpdateRequestDto motUpdateRequest)
    {
        try
        {
            MotUpdateRequest updateRequest = _mapper.Map<MotUpdateRequest>(motUpdateRequest);
            MotUpdateResponse motUpdateResponse = await _motUpdatesService.GetMotUpdatesAsync(updateRequest).ConfigureAwait(false);
            return motUpdateResponse != null ? Ok(motUpdateResponse) :
                     BadRequest(new ApiErrorResponse(statusCode: (int)HttpStatusCode.NotFound, new string[] { JsonSerializer.Serialize(motUpdateResponse) }));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiErrorResponse { Message = String.Format("{0} {1}",ex.Message, ex.InnerException.Message), StatusCode = (int)HttpStatusCode.InternalServerError });
        }
    }
    [HttpGet("GetMotUpdateLists")]
    public async Task<ActionResult<object>> GetMotUpdateLists()
    {
        try
        {
            var motUpdateResponse = await _motUpdatesService.GetMotUpdateListsAsync().ConfigureAwait(false);
            return motUpdateResponse != null ? Ok(motUpdateResponse) :
                 BadRequest(new ApiErrorResponse(statusCode: (int)HttpStatusCode.NotFound, new string[] { JsonSerializer.Serialize( motUpdateResponse) }));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiErrorResponse { Message = String.Format("{0} {1}", ex.Message, ex.InnerException.Message), StatusCode = (int)HttpStatusCode.InternalServerError });
        }
    }
    [HttpGet("GetMotUpdatesByTrainStation")]
    public async Task<ActionResult<ResponseMotUpdatesByTrainStationDto>> GetMotUpdatesByTrainStation([FromQuery] int stationId, DateTime startTime)
    {
                      try
        {
            ResponseMotUpdatesByTrainStationDto motUpdateResponse = await _motUpdatesService.GetMotUpdatesByTrainStationsAsync(stationId, startTime).ConfigureAwait(false);
            return motUpdateResponse!=null? Ok(motUpdateResponse):
             BadRequest(new ApiErrorResponse(statusCode: (int)HttpStatusCode.BadRequest, new string[] {String.Format( "Invalid parameters: {0} {1}", stationId, startTime) }));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiErrorResponse { Message = String.Format("{0} {1}", ex.Message, ex.InnerException.Message), StatusCode = (int)HttpStatusCode.InternalServerError });
        }
    }
    [HttpGet("GetMotConvertion")]
    public async Task<ActionResult<IEnumerable<MotConvertion>>> GetMotConvertion()
    {
                      try
        {
            var motConvertionResponse = await _motUpdatesService.GetMotConvertionListContentAsync().ConfigureAwait(false);
            return Ok(motConvertionResponse);
           
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiErrorResponse { Message = String.Format("{0} {1}", ex.Message, ex.InnerException.Message), StatusCode = (int)HttpStatusCode.InternalServerError });
        }
    }
}
