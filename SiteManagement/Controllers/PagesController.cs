using AutoMapper;
using Core.Entities.PagesManagement;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteManagement.Dtos;

namespace SiteManagement.Controllers;

public class PagesController : BaseApiController
{
    private readonly IPagesService _pagesService;
    private readonly IMapper _mapper;

    public PagesController(IPagesService pagesService, IMapper mapper)
    {
        _mapper = mapper;
        _pagesService = pagesService;
    }


    [HttpGet]
    [Authorize(Policy = "PageRole")]
    public async Task<IEnumerable<PageDto>> GetPages()
    {
        IEnumerable<Page> pages = await _pagesService.GetPagesAsync();
        IEnumerable<PageDto> pagesDto = _mapper.Map<IEnumerable<PageDto>>(pages);
        return pagesDto;
    }

    [HttpGet("GetPagesPerUser")]
    [Authorize]
    public async Task<IEnumerable<PageDto>> GetPagesPerUser(string userEmail)
    {
        IEnumerable<Page> pages = await _pagesService.GetPagesAsync();
        IEnumerable<PageDto> pagesDto = _mapper.Map<IEnumerable<PageDto>>(pages);
        return pagesDto;
    }
                     
    [HttpPost]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> AddPage(Page request)
    {
        try
        {
            bool res = await _pagesService.AddPageAsync(request);
            return Ok(res);
        }
        catch
        {
            return BadRequest(new ApiErrorResponse { Message = "שמירה נכשלה", StatusCode = 400 });
        }
    }

    [HttpPut]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> UpdatePage(Page request)
    {
        try
        {
            bool res = await _pagesService.UpdatePageAsync(request);
            return Ok(res);
        }
        catch
        {
            return BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשלה", StatusCode = 400 });
        }
    }
    [HttpDelete("{id}")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> deletePage(int id)
    {
        try
        {
            bool res = await _pagesService.DeletePageAsync(id);
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
