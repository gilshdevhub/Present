using AutoMapper;
using Core.Entities.Translation;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteManagement.Dtos;

namespace SiteManagement.Controllers;

public class TranslationController : BaseApiController
{
    private readonly ITranslationService _translationService;
    private readonly IMapper _mapper;

    public TranslationController(ITranslationService translationService, IMapper mapper)
    {
        _translationService = translationService;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize]//(Policy = "PageRole")]
    public async Task<IEnumerable<TranslationResponseDto>> GetAllTranslationsAsync()
    {
        IEnumerable<Translation> translations = await _translationService.GetAllTranslationsAsync();
        IEnumerable<TranslationResponseDto> result = _mapper.Map<IEnumerable<Translation>, IEnumerable<TranslationResponseDto>>(translations);
        return result;
    }

    [HttpGet("getitem")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<TranslationResponseDto>> GetTranslation([FromQuery] ConfigurationCriteriaDto request)
    {
        ActionResult result;

        Translation translation = await _translationService.GetItemAsync(request.Key, request.SystemTypeId);

        if (translation == null)
        {
            result = NotFound();
        }
        else
        {
            TranslationResponseDto translationResponseDto = _mapper.Map<TranslationResponseDto>(translation);
            result = Ok(translationResponseDto);
        }

        return result;
    }

    [Authorize(Policy = "PageRole")]
    [HttpPost]
    public async Task<ActionResult<TranslationResponseDto>> AddTranslation(TranslationRequestDto translate)
    {
        ActionResult<TranslationResponseDto> result;

        Translation translateToSave = _mapper.Map<Translation>(translate);

        try
        {
            Translation newTranslation = await _translationService.AddTranslationAsync(translateToSave);
            TranslationResponseDto transDto = _mapper.Map<TranslationResponseDto>(newTranslation);
            result = Ok(transDto);
        }
        catch(Exception ex)
        {
            result = ex.InnerException.HResult==-2146232060? 
                    BadRequest(new ApiErrorResponse { Message = "תרגום עם המפתח המבוקש, כבר קיים.", StatusCode = 409 }):
                    BadRequest(new ApiErrorResponse { Message = "שמירה נכשלה.", StatusCode = 400 });
        }

        return result;
    }

    [Authorize(Policy = "PageRole")]
    [HttpPut]
    public async Task<ActionResult<bool>> UpdateTranslation(TranslationResponseDto translate)
    {
        ActionResult result;

        Translation translateToUpdate = await _translationService.GetItemByIdAsync(translate.Id);
        if (translateToUpdate == null)
        {
            result = BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשלה", StatusCode = 400 });
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

            bool success = await _translationService.UpdateTranslationAsync(translateToUpdate);
            result = Ok(success);
        }

        return result;
    }
    [HttpDelete("{Id}")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> deleteTranslation(int Id)
    {
        var error = new ClientErrorData();
        ActionResult<bool> result;
        try
        {
            bool res = await _translationService.DeleteAsync(Id);
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
