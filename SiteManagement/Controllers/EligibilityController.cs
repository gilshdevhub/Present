using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteManagement.Dtos;

namespace SiteManagement.Controllers;

public class EligibilityController : BaseApiController
{
    private readonly IEligibilityService _eligibilityService;
    public EligibilityController(IEligibilityService eligibilityService)
    {
        _eligibilityService = eligibilityService;
    }

    [HttpPost]
    [Authorize(Policy = "PageRole")]
    public async Task<bool> UploadFile([FromForm] UploadFile uploadFile)//IFormFile FileToLoad )
    {
        bool result = await _eligibilityService.UploadFileAsync(uploadFile.FileToLoad);
        return result;
    }

  
}
