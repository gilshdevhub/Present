using Core.Entities;
using Core.Filters;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;

[ServiceFilter(typeof(WriteToLogFilterAttribute))]
public class BackgroundImageController : BaseApiController
{
    private readonly IBackgroundImageService _backgroundImageService;

    public BackgroundImageController(IBackgroundImageService backgroundImageService)
    {
        _backgroundImageService = backgroundImageService;
    }

    [HttpGet("DownloadFile")]
    public async Task<ActionResult<Dictionary<string, BGResponse>>> DownloadFileToStorage()
    {
        try
        {
            var fileBytes = await _backgroundImageService.DownloadBackgroundFilesAsync();
            return Ok(fileBytes);
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
