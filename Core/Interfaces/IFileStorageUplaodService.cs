using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.Interfaces;

public interface IFileStorageUplaodService
{
    Task UploadFileAsync(UploadFile file, Guid guid, string type);
    Task UploadBackGroundAsync(UploadBackGround uploadFile);
    Task<byte[]> DownloadBackgroundFileAsync(string? imageName="");
    Task<byte[]> DownloadFileAsync(string fileName, Guid guid, string tenderType);
    Task<ActionResult<List<string>>> GetFilesListAsync(Guid guid, string tenderType);
    Task<bool> DeleteTendersDocumentsAsync(TenderDocuments tenderDocuments);
}
