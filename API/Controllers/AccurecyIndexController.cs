using AutoMapper;
using AutoMapper.Internal;
using Core.Entities.AccurecyIndex;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Controllers;

public class AccurecyIndexController : BaseApiController
{
    private readonly IAccurecyIndexService _accurecyIndexService;

    public AccurecyIndexController(IAccurecyIndexService accurecyIndexService)
    {
        _accurecyIndexService = accurecyIndexService;
    }
    [HttpGet("GetAccurecyIndexData")]
    public async Task<ActionResult<IEnumerable<AccurecyIndexFilteredData>>> GetAccurecyIndexData()
    {
        try
        {
            AccurecyIndexFilteredData accurecyIndex = await _accurecyIndexService.GetAccurecyIndexDataAsync().ConfigureAwait(false);
                       return accurecyIndex != null ? Ok(accurecyIndex) :
                 BadRequest(new ApiErrorResponse(statusCode: (int)HttpStatusCode.BadRequest, new string[] { JsonSerializer.Serialize(accurecyIndex) }));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiErrorResponse { Message = ex.Message, StatusCode = (int)HttpStatusCode.InternalServerError });
        }
    }
}
