using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ContentManagement.Controllers;

public class UmbracoController : BaseApiController
{
    private readonly IConfiguration _configuration;
    private readonly IUmbracoService _umbracoService;

    public UmbracoController(IConfiguration configuration, IUmbracoService umbracoService)
    {
        _configuration = configuration;
        _umbracoService = umbracoService;
    }

    [HttpGet]
    public async Task<string> GetSiteContent(string culture = "he")
    {
        string umbracoUrl = _configuration.GetSection("UmbracoUrl").Value;
        var content = JsonConvert.SerializeObject(await _umbracoService.GetCacheContentAsync(culture, umbracoUrl));
        return content;
    }

    [HttpGet("Clean")]
    public async Task<ActionResult> GetContent(string culture = "he")
    {
        string umbracoUrl = _configuration.GetSection("UmbracoUrl").Value;
        var content = await _umbracoService.GetCleanContent(culture, umbracoUrl);//JsonConvert.SerializeObject(await _umbracoService.GetCleanContent(culture, umbracoUrl));
        return Ok(content);
    }

    [HttpGet("Search")]
    public async Task<string> Search(string term, string culture = "he")
    {
        string umbracoUrl = _configuration.GetSection("UmbracoUrl").Value;
        if (string.IsNullOrEmpty(term)) { return "No search term."; }
        if (string.IsNullOrEmpty(umbracoUrl)) { return "Search url missing."; }
        try
        {
            var content = JsonConvert.SerializeObject(await _umbracoService.Search(term, culture, umbracoUrl));
            return content;
        }
        catch (Exception ex) 
        {
            return ex.Message;
        }

     
    }
}

