using API.Dtos.URLTranslations;
using API.Filters;
using AutoMapper;
using Core.Entities;
using Core.Filters;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Controllers;

[ServiceFilter(typeof(WriteToLogFilterAttribute))]
public class URLTranslationsController : BaseApiController
{
    private readonly IURLTranslationService _urlTranslationsService;
    private readonly IMapper _mapper;

    public URLTranslationsController(IURLTranslationService urlTranslationsService, IMapper mapper)
    {
        _urlTranslationsService = urlTranslationsService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<string>> GetURLTranslations1([FromQuery] URLTranslationRequestDto request)
    {
        URLTranslationRequest urlTranslationRequest = _mapper.Map<URLTranslationRequest>(request);
        JObject urlTranslations = await _urlTranslationsService.GetActiveURLTranslations1Async(urlTranslationRequest).ConfigureAwait(false);
        return Ok(JsonConvert.SerializeObject(urlTranslations));
    }

    [HttpGet("bylanguage")]
    [URLTranslationLanguageFilter]
    public async Task<ActionResult<string>> GetURLTranslationsByLanguegeId1([FromQuery] URLTranslationRequestDto request)
    {
        URLTranslationRequest urlTranslationRequest = _mapper.Map<URLTranslationRequest>(request);
        JObject urlTranslations = await _urlTranslationsService.GetActiveURLTranslations2Async(urlTranslationRequest)
            .ConfigureAwait(false);
        return Ok(JsonConvert.SerializeObject(urlTranslations));
    }
}
