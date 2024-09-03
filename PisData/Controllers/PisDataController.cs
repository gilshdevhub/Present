using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PisData.Dtos;
using Core.Entities.PisData;
using System.Xml.Serialization;
using Core.Errors;
using System.Net;
using Infrastructure.Services;

namespace PisData.Controllers;

public class PisDataController : BaseApiController
{
    private readonly IPisData _pisData;

    public PisDataController(IPisData pisData)
    {
        _pisData = pisData;
    }
    [HttpGet("GetPisDataByStationId/{stationId}")]
    public async Task<ActionResult<IEnumerable<VehicleActivity>>> GetPisDataByStationId(int stationId)
    {
        IEnumerable<VehicleActivity> result;
        try
        {
            result = await _pisData.GetPisDataByStationIdAsync(stationId);
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex);
        }
        return Ok(result);
    }
    [HttpPost("SetPisDataCacheAsync")]
    public async Task<ActionResult<bool>> SetPisDataCache(PisDataInsertDto dataInsertDto)//[FromBody] string xml)
    {
        if(string.IsNullOrEmpty( dataInsertDto.XmlData) || string.IsNullOrWhiteSpace( dataInsertDto.XmlData) )
            return new BadRequestObjectResult(false);
        string data = dataInsertDto.XmlData;
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Siri));
            using (StringReader reader = new StringReader(data.Replace(@"xmlns=""http://www.siri.org.uk/siri""", @"xmlns:ns1=""http://www.siri.org.uk/siri""")))
            {
                var pisData = (Siri)serializer.Deserialize(reader);
                var result = await _pisData.SetPisDataCacheAsync(pisData);
                return Ok(result);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiErrorResponse(statusCode: (int)HttpStatusCode.BadRequest, ex.Message));
        }
    }
}
