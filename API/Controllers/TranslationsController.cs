using API.Dtos.Translations;
using API.Filters;
using AutoMapper;
using Core.Entities.Translation;
using Core.Filters;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Controllers;

[ServiceFilter(typeof(WriteToLogFilterAttribute))]
public class TranslationsController : BaseApiController
{
    private readonly ITranslationService _translationsService;
    private readonly IMapper _mapper;

    public TranslationsController(ITranslationService translationsService, IMapper mapper)
    {
        _translationsService = translationsService;
        _mapper = mapper;
    }

                     
                        
    [HttpGet]
    public async Task<ActionResult<string>> GetTranslations1([FromQuery] TranslationRequestDto request)
    {
        TranslationRequest translationRequest = _mapper.Map<TranslationRequest>(request);
        JObject translations = await _translationsService.GetActiveTranslations1Async(translationRequest).ConfigureAwait(false);
        return Ok(JsonConvert.SerializeObject(translations));
    }

    [HttpGet("bylanguage")]
    [TranslationLanguageFilter]
    public async Task<ActionResult<string>> GetTranslationsByLanguegeId1([FromQuery] TranslationRequestDto request)
    {
        TranslationRequest translationRequest = _mapper.Map<TranslationRequest>(request);
        JObject translations = await _translationsService.GetActiveTranslations2Async(translationRequest)
            .ConfigureAwait(false);
        return Ok(JsonConvert.SerializeObject(translations));
    }
}
