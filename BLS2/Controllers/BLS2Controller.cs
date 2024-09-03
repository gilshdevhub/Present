using AutoMapper;
using BLS2.Dtos;
using Core.Entities.MotUpdates;
using Core.Errors;
using Core.Filters;
using Core.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace BLS2.Controllers;
[ServiceFilter(typeof(WriteToLogFilterAttribute))]
public class BLS2Controller : BaseApiController
{
    private readonly IBLS2Service _bLS2Service;

    public BLS2Controller(IBLS2Service bLS2Service)
    {
        _bLS2Service = bLS2Service;
    }

   
    [HttpGet("GetSiri")]
    public async Task<ActionResult<MotUpdateResponse>> GetSiri([FromQuery] string motUrl)
    {
        try
        {
            MotUpdateResponse motUpdateResponse = await _bLS2Service.GetSiri(motUrl).ConfigureAwait(false);
            return motUpdateResponse != null ? Ok(motUpdateResponse) :
             BadRequest(new ApiErrorResponse(statusCode: (int)HttpStatusCode.BadRequest, new string[] { String.Format("Invalid parameters: {0}", motUrl) }));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiErrorResponse { Message = String.Format("{0} {1}", ex.Message, ex.InnerException.Message), StatusCode = (int)HttpStatusCode.InternalServerError });
        }
    }

}
