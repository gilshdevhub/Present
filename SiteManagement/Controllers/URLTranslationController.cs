using AutoMapper;
using Core.Entities;
using Core.Entities.Configuration;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteManagement.Dtos;

namespace SiteManagement.Controllers;

public class URLTranslationController : BaseApiController
{
    private readonly IURLTranslationService _urlTranslationService;
    private readonly IMapper _mapper;

    public URLTranslationController(IURLTranslationService urlTranslationService, IMapper mapper)
    {
        _urlTranslationService = urlTranslationService;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize]//(Policy = "PageRole")]
    public async Task<IEnumerable<URLTranslationResponseDto>> GetAllURLTranslationsAsync()
    {
        IEnumerable<URLTranslation> urlTranslations = await _urlTranslationService.GetAllURLTranslationsAsync();
        IEnumerable<URLTranslationResponseDto> result = _mapper.Map<IEnumerable<URLTranslation>, IEnumerable<URLTranslationResponseDto>>(urlTranslations);
        return result;
    }

    [HttpGet("getitem")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<URLTranslationResponseDto>> GetURLTranslation([FromQuery] ConfigurationCriteriaDto request)
    {
        ActionResult result;

        URLTranslation urlTranslation = await _urlTranslationService.GetItemAsync(request.Key, request.SystemTypeId);

        if (urlTranslation == null)
        {
            result = NotFound();
        }
        else
        {
            URLTranslationResponseDto urlTranslationResponseDto = _mapper.Map<URLTranslationResponseDto>(urlTranslation);
            result = Ok(urlTranslationResponseDto);
        }

        return result;
    }

    [Authorize(Policy = "PageRole")]
    [HttpPost]
    public async Task<ActionResult<bool>> AddURLTranslation(URLTranslationRequestDto translate)
    {
        URLTranslation translateToSave = _mapper.Map<URLTranslation>(translate);

        try
        {
            URLTranslation newURLTranslation = await _urlTranslationService.AddURLTranslationAsync(translateToSave);
            if (newURLTranslation == null)
            {
                return BadRequest(new ApiErrorResponse { Message = "קישור עם שם כזה כבר קיים לסביבה זו", StatusCode = 400 });
            }
            else
            {
                return Ok(true);
            }
            
        }
        catch(Exception ex) 
        {
            return BadRequest(new ApiErrorResponse { Message = "הוספה נכשלה", StatusCode = 400 });
        }
    }

    [Authorize(Policy = "PageRole")]
    [HttpPut]
    public async Task<ActionResult<bool>> UpdateURLTranslation(URLTranslationResponseDto translate)
    {
        URLTranslation translateToUpdate = await _urlTranslationService.GetItemByIdAsync(translate.Id);
        if (translateToUpdate == null)
        {
            return BadRequest(new ApiErrorResponse { Message = "עדכון נכשל", StatusCode = 400 });
        }
        else
        {
            translateToUpdate.Description = translate.Description;
            translateToUpdate.Arabic = translate.Arabic;
            translateToUpdate.English = translate.English;
            translateToUpdate.Hebrew = translate.Hebrew;
            translateToUpdate.Russian = translate.Russian;
            translateToUpdate.SystemTypeId = translate.SystemTypeId;
            translateToUpdate.IsActive = translate.IsActive;

            bool success = await _urlTranslationService.UpdateURLTranslationAsync(translateToUpdate);
            if (success)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest(new ApiErrorResponse { Message = "עדכון נכשל", StatusCode = 400 });
            }
        }
    }
    [HttpDelete("{Id}")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<IEnumerable<ConfigurationParameter>>> deleteURLTranslation(int Id)
    {
        try
        {
            bool res = await _urlTranslationService.DeleteAsync(Id);
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
