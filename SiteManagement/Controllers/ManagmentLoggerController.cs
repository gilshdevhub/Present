using Core.Entities.ManagmentLogger;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteManagement.Dtos;
using System.Text.Json;

namespace SiteManagement.Controllers;

public class ManagmentLoggerController : BaseApiController
{
    private readonly IManagmentLogger _managmentLogger;
    public ManagmentLoggerController(IManagmentLogger managmentLogger)
    {
        _managmentLogger = managmentLogger;
    }
    [HttpGet("GetLogsBetweenDates")]
    [Authorize]
    public async Task<IEnumerable<ManagmentLogDto>> GetLogsBetweenDates(DateTime startDate, DateTime endDate)
    {
        IEnumerable<ManagmentLog> AllManagmentLog = await _managmentLogger.GetAllLogAsync();
        IEnumerable<ManagmentLogDto> result = JsonSerializer.Deserialize<IEnumerable<ManagmentLogDto>>(JsonSerializer.Serialize(AllManagmentLog.Where(x => x.LogTime >= startDate && x.LogTime < endDate).ToList()));
        return result;
    }
    [HttpGet("GetLogById")]
    [Authorize]
    public async Task<ManagmentLogDto> GetLogById(int Id)
    {
        IEnumerable<ManagmentLog> AllManagmentLog = await _managmentLogger.GetAllLogAsync();
        ManagmentLogDto result = JsonSerializer.Deserialize<ManagmentLogDto>(JsonSerializer.Serialize(AllManagmentLog.FirstOrDefault(x => x.Id == Id)));
        return result;
    }
    [HttpDelete("DeleteLog")]
    [Authorize(Policy = "PageRole")]
   [ApiExplorerSettings(IgnoreApi = true)]
       public async Task<bool> DeleteLog(int Id)
    {
        bool result = await _managmentLogger.DeleteLogAsync(Id);
        return result;
    }
    [HttpPost("AddLog")]
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ManagmentLogDto> AddLog(ManagmentLogDtoPost NewManagmentLog)
    {
        ManagmentLog NewManagmentLogEnt = JsonSerializer.Deserialize<ManagmentLog>(JsonSerializer.Serialize(NewManagmentLog));
        ManagmentLog resultEnt = await _managmentLogger.AddLogAsync(NewManagmentLogEnt);
        ManagmentLogDto result = JsonSerializer.Deserialize<ManagmentLogDto>(JsonSerializer.Serialize(resultEnt));
        return result;
    }

}
