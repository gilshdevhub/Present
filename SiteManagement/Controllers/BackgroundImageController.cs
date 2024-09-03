using AutoMapper;
using SiteManagement.Dtos;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using System.Net;
using Core.Errors;

namespace SiteManagement.Controllers;

public class BackgroundImageController : BaseApiController
{
    private readonly IBackgroundImageService _backgroundImageService;

    public BackgroundImageController(IBackgroundImageService  backgroundImageService)
    {
        _backgroundImageService = backgroundImageService;
    }

    [HttpGet]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<List<BackgroundImageDto>>> DownloadFileToStorage()
    {
        try
        {
           return await _backgroundImageService.DownloadAllBackgroundFilesAsync();
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [HttpPost("UploadFile")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> UploadFileToStorage([FromForm] UploadBackGround uploadFile)
    {
        if (uploadFile.FileToLoad != null)
            await _backgroundImageService.UploadBackGroundAsync(uploadFile);
        try
        {
            await _backgroundImageService.UpdateDatesAsync(uploadFile.From, uploadFile.Untill, uploadFile.Name);

        }
        catch (Exception ex)
        {

            throw;
        }
        return Ok(true);
    }

    [HttpPost("UpdateDates")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> UpdateDates([FromForm] UploadBackUpadetDateGround uploadFile)
    {
        
        try
        {
            await _backgroundImageService.UpdateDatesAsync(uploadFile.From, uploadFile.Untill, uploadFile.Name);

        }
        catch (Exception ex)
        {
            return BadRequest(new ApiErrorResponse { Message = String.Format("{0} {1}", ex.Message, ex.InnerException.Message), StatusCode = (int)HttpStatusCode.InternalServerError });
        }
        return Ok(true);
    }

    [HttpDelete("{name}")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> Delete(string name)
    {

        await _backgroundImageService.DeleteAsync(name);

        return Ok(true);
    }


}
