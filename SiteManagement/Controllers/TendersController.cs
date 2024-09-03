using AutoMapper;
using Core.Entities;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteManagement.Dtos;
using System.Text.Json;

namespace SiteManagement.Controllers;

public class TendersController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly ITendersService _tendersService;

    public TendersController(ITendersService tendersService, IMapper mapper)
    {
        _mapper = mapper;
        _tendersService = tendersService;
    }

    [HttpGet("{categoryId}")]
    [Authorize(Policy = "PageRole")]
    public async Task<IEnumerable<TendersDto>> GetAppTenders(int categoryId)
    {
        IEnumerable<TendersDto> tenders = await _tendersService.GetTendersAsync(categoryId);
        return tenders;
    }
    [HttpGet]
    [Authorize(Policy = "PageRole")]
    public async Task<IEnumerable<TendersDto>> GetAppTenders()
    {
        IEnumerable<TendersDto> tenders = await _tendersService.GetTendersAsync();
        return tenders;
    }

    [HttpGet("IsNumberExist/{Id}")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> IsNumberExist(int Id)
    {
        try
        {
            bool result = await _tendersService.TenderNumberExist(Id);
            return Ok(result);
        }catch (Exception ex)
        {
            return BadRequest(new ApiErrorResponse { Message = "חיפוש מספר מכרז נכשל.", StatusCode = 400 });
        }
    }

    [HttpPost]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<Guid>> AddTenders(TendersPostDto request)
    {
        ActionResult<Guid> result;

        Tenders requseEnt = _mapper.Map<Tenders>(request);

        try
        {
            Tenders responseENT = await _tendersService.AddTendersAsync(requseEnt);
            Guid tenderId = responseENT.Id;
            
            result = Ok(responseENT.Id);
        }
        catch (Exception ex)
        {
            result = BadRequest(new ApiErrorResponse { Message = "שמירה נכשלה", StatusCode = 400 });
        }

        return result;
    }

    [HttpPut]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<TendersDto>> UpdateTenders(TendersDto request)//, string PageRole)
    {
        ActionResult<TendersDto> result;

        Tenders requseEnt = _mapper.Map<Tenders>(request);

        try
        {
            Tenders responseENT = await _tendersService.UpdateTendersAsync(requseEnt);
            TendersDto res = JsonSerializer.Deserialize<TendersDto>(JsonSerializer.Serialize(responseENT));
            result = Ok(res);
        }
        catch (Exception ex)
        {
            result = BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשלה", StatusCode = 400 });
        }

        return result;
    }
    [HttpDelete("{id}")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> deleteTenders(Guid id)
    {
        var error = new ClientErrorData();
        ActionResult<bool> result;
        try
        {
            bool res = await _tendersService.DeleteTendersAsync(id);
            if (res)
            {
                result = Ok(true);
            }
            else
            {
                result = BadRequest(new ApiErrorResponse { Message = "מחיקה נכשלה", StatusCode = 400 });
            }
        }
        catch
        {
            result = BadRequest(new ApiErrorResponse { Message = "מחיקה נכשלה", StatusCode = 400 });
        }

        return result;
    }
}
