using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SiteManagement.Controllers;

public class TenderDocumentsController : BaseApiController
{
    private readonly IFileStorageUplaodService _fileStorageUplaodService;
    private readonly IMapper _mapper;
    public TenderDocumentsController(IFileStorageUplaodService fileStorageUplaodService, IMapper mapper)
    {
        _fileStorageUplaodService = fileStorageUplaodService;
        _mapper = mapper;
    }

    [HttpPost("UploadFile")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> UploadFileToStorage([FromForm] UploadFile uploadFile, Guid guid, string type)
    {
        if (uploadFile.FileToLoad != null)
            await _fileStorageUplaodService.UploadFileAsync(uploadFile, guid, type);
        return Ok(true);
    }
    [HttpGet("DownloadFile")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult> DownloadFileFromStorage(string fileName, Guid guid, string tenderType)
    {
        var fileBytes = await _fileStorageUplaodService.DownloadFileAsync(fileName, guid, tenderType);
        return new FileContentResult(fileBytes, "application/octet-stream")
        {
            FileDownloadName = fileName,
        };
    }
    [HttpGet("FilesByGuid")]
    [Authorize(Policy = "PageRole")]
    public async Task<IActionResult> FilesByGuid(Guid guid, string tenderType)
    {
        var fileBytes = await _fileStorageUplaodService.GetFilesListAsync(guid, tenderType);
        return Ok(fileBytes.Value);
    }

    [HttpDelete]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> Delete(TenderDocumentsDto tenderDocumentsDto)
    {
        TenderDocuments tenderDocuments = _mapper.Map<TenderDocuments>(tenderDocumentsDto);
        var res = await _fileStorageUplaodService.DeleteTendersDocumentsAsync(tenderDocuments);
        return Ok(res);
    }
}
