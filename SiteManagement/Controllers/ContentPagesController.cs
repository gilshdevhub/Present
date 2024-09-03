using AutoMapper;
using Core.Entities.ContentPages;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SiteManagement.Controllers;

public class ContentPagesController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IContentPagesService _contentPagesService;

    public ContentPagesController(IContentPagesService contentPagesService, IMapper mapper)
    {
        _mapper = mapper;
        _contentPagesService = contentPagesService;
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ContentPages> GetContentPage(int id)
    {
        ContentPages station = await _contentPagesService.GetContentPageAsync(id);
        ContentPages stationDto = _mapper.Map<ContentPages>(station);
        return stationDto;
    }

    [HttpGet]
    [Authorize(Policy = "PageRole")]
    public async Task<IEnumerable<ContentPages>> GetContentPages()
    {
        IEnumerable<ContentPages> contentPages = await _contentPagesService.GetContentPagesAsync();
        return contentPages;
    }

    [HttpPost]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<ContentPages>> AddContentPage(ContentPages request)
    {
        ContentPages station = _mapper.Map<ContentPages>(request);

        try
        {
            ContentPages newContentPage = await _contentPagesService.AddContentPageAsync(station);
            ContentPages stationDto = _mapper.Map<ContentPages>(newContentPage);
            return Ok(stationDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiErrorResponse { Message = "שמירה נכשלה", StatusCode = 400 });
        }
    }

    [HttpPut]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<ContentPages>> updateContentPage(ContentPages request)
    {
        ActionResult<ContentPages> result;

        ContentPages station = _mapper.Map<ContentPages>(request);

        try
        {
            ContentPages newContentPage = await _contentPagesService.UpdateContentPageAsync(station);
            ContentPages stationDto = _mapper.Map<ContentPages>(newContentPage);
            return Ok(stationDto);
        }
        catch
        {
            return BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשלה", StatusCode = 400 });
        }
    }
    [HttpDelete("{Id}")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> deleteContentPage(int Id)
    {
        var error = new ClientErrorData();
        ActionResult<bool> result;
        try
        {
            bool res = await _contentPagesService.DeleteContentPageAsync(Id);
            if (res)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest(new ApiErrorResponse { Message = "מחיקה נכשלה", StatusCode = 400 });
            }
        }
        catch
        {
            return BadRequest(new ApiErrorResponse { Message = "מחיקה נכשלה", StatusCode = 400 });
        }
    }
}
