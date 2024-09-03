using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using System;

namespace Infrastructure.Services;
public class FileStorageUplaodService : IFileStorageUplaodService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IConfiguration _configuration;
    private readonly ITendersService _tendersService;
    private readonly IExemptionNotices _exemptionNotices;
    private readonly ISingleSupplierIService _singleSupplierIService;
    private readonly IPlanningAndRatesService _planningAndRatesService;
    private readonly RailDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly ITendersCommonService _tendersCommonService;

    public readonly Dictionary<string, string> map = new(StringComparer.OrdinalIgnoreCase)
    {
        [".doc"] = "application/msword",
        [".docx"] = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        [".pdf"] = "application/pdf",
        [".txt"] = "text/*",
        [".xls"] = "application/vnd.ms-excel",
        [".xlsx"] = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        [".rar"] = "application/vnd.rar",
        [".zip"] = "application/zip"
    };
    public FileStorageUplaodService(BlobServiceClient blobServiceClient,
        IConfiguration configuration, ITendersService tendersService,
        IExemptionNotices exemptionNotices, ISingleSupplierIService singleSupplierIService,
        RailDbContext context, ICacheService cacheService, IPlanningAndRatesService planningAndRatesService, ITendersCommonService tendersCommonService)
    {
        _blobServiceClient = blobServiceClient;
        _configuration = configuration;
        _tendersService = tendersService;
        _exemptionNotices = exemptionNotices;
        _singleSupplierIService = singleSupplierIService;
        _context = context;
        _cacheService = cacheService;
        _planningAndRatesService = planningAndRatesService;
        _tendersCommonService = tendersCommonService;
    }

    public async Task<byte[]> DownloadFileAsync(string fileName, Guid guid, string tenderType)
    {
                      var blobContainer = _blobServiceClient.GetBlobContainerClient("tenders");
        if (!blobContainer.Exists())
        {
            return null;
        }
        var blobClient = blobContainer.GetBlobClient($"{tenderType}/{guid}/{fileName}");
        var downloadContent = await blobClient.DownloadAsync();
        using MemoryStream ms = new();
        await downloadContent.Value.Content.CopyToAsync(ms);
        return ms.ToArray();
    }

    public async Task<byte[]> DownloadBackgroundFileAsync(string? imageName="")
    {
        string image = String.IsNullOrEmpty(imageName) ? "background.jpeg" : imageName;
        var blobContainer = _blobServiceClient.GetBlobContainerClient("background");
        if (!blobContainer.Exists())
        {
            return null;
        }
        var blobClient = blobContainer.GetBlobClient(image);
        if (!await blobClient.ExistsAsync())
        {
            return null;
        }
        var cancellationTokenSource = new CancellationTokenSource(10000);
        var cancellationToken = cancellationTokenSource.Token;
        try
        {
            
            var downloadStream = new MemoryStream();
            await blobClient.DownloadToAsync(downloadStream, cancellationToken: cancellationToken);
            downloadStream.Position = 0;
            return downloadStream.ToArray();
                                                    }
        catch ( Exception ex )
        {
            return null;
        }
       
    }

    public async Task UploadFileAsync(UploadFile file, Guid guid, string type)
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient("tenders");
        if (!blobContainer.Exists())
        {
            _ = blobContainer.Create();
        }

        TenderDocuments newDoc = new();

        newDoc.DocDisplay = file.DocDisplay;
        newDoc.DocType = file.DocType;
        string pathToFile = $"{guid}/{file.FileToLoad.FileName}";

        switch (type)
        {
            case "Tenders":
            case "AllTenders":
                newDoc.TendersId = guid;
                pathToFile = "AllTenders/" + pathToFile;
                newDoc.DocName = "tenders/" + pathToFile;
                break;
            case "SingleSupplier":

                newDoc.SingleSupplierId = guid;
                pathToFile = "SingleSupplier/" + pathToFile;
                newDoc.DocName = "tenders/" + pathToFile;
                break;
            case "ExemptionNotices":

                newDoc.ExemptionNoticesId = guid;
                pathToFile = "ExemptionNotices/" + pathToFile;
                newDoc.DocName = "tenders/" + pathToFile;
                break;
            case "PlanningAndRates":
                newDoc.PlanningAndRatesId = guid;                pathToFile = "PlanningAndRates/" + pathToFile;                newDoc.DocName = "tenders/" + pathToFile;
                break;
            default:
                newDoc.TendersId = guid;
               
                pathToFile = "AllTenders/" + pathToFile;
                newDoc.DocName = "tenders/" + pathToFile;
                break;
        }

        _ = await AddTendersDocumentsAsync(newDoc);

        switch (type)
        {
            case "Tenders":
            case "AllTenders":
                _ = await _tendersService.UpdateCacheTendersAsync();
                break;
            case "SingleSupplier":
                _ = await _singleSupplierIService.UpdateCacheSingleSupplierAsync();
                break;
            case "ExemptionNotices":
                _ = await _exemptionNotices.UpdateCacheExemptionNoticesAsync();
                break;
            case "PlanningAndRates":
                _ = await _planningAndRatesService.UpdateCachePlanningAndRatesAsync();
                break;
            default:

                break;
        }

        var blobClient = blobContainer.GetBlobClient(pathToFile);
        using Stream fs = file.FileToLoad.OpenReadStream();
        string ext = Path.GetExtension(file.FileToLoad.FileName);

        BlobUploadOptions options = new BlobUploadOptions
        {
            TransferOptions = new StorageTransferOptions
            {
                               MaximumTransferSize = 104857600
            }
        };
        if (map.TryGetValue(ext, out string contentType))
        {
            options.HttpHeaders = new BlobHttpHeaders { ContentType = contentType };
            _ = await blobClient.UploadAsync(fs, options);
        }
    }
    public async Task UploadBackGroundAsync(UploadBackGround uploadFile)
    {
        IFormFile file = uploadFile.FileToLoad;
        var blobContainer = _blobServiceClient.GetBlobContainerClient("background");
        if (!blobContainer.Exists())
        {
            _ = blobContainer.Create();
        }


        string pathToFile = $"background.jpeg";


        var blobClient = blobContainer.GetBlobClient(pathToFile);
        using Stream fs = file.OpenReadStream();
        string ext = Path.GetExtension(pathToFile);

        BlobUploadOptions options = new BlobUploadOptions
        {
            TransferOptions = new StorageTransferOptions
            {
                               MaximumTransferSize = 104857600
            }
        };
        if (map.TryGetValue(ext, out string contentType))
        {
            options.HttpHeaders = new BlobHttpHeaders { ContentType = contentType };
            _ = await blobClient.UploadAsync(fs, options);
        }
    }

    public async Task<ActionResult<List<string>>> GetFilesListAsync(Guid guid, string tenderType)
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient("tenders");
        if (!blobContainer.Exists())
            return null;

        string prefixString = $"{tenderType}/{guid}";
        List<string> result = new();
        try
        {
            var resultSegment = blobContainer.GetBlobsAsync().AsPages(default, null);
            await foreach (var blobPage in resultSegment)
            {
                foreach (BlobItem blobItem in blobPage.Values)
                {
                    if (blobItem.Name.Contains(prefixString))
                        result.Add(blobItem.Name.Replace(prefixString + "/", ""));
                }
            }
        }
        catch (RequestFailedException e)
        {
            return new BadRequestObjectResult(e.Message);
            throw;
        }
        return result;
    }

    public async Task<TenderDocuments?> AddTendersDocumentsAsync(TenderDocuments tenderDocument)
    {
        EntityEntry<TenderDocuments> entity = await _context.TenderDocuments.AddAsync(tenderDocument).ConfigureAwait(false);

        TenderDocuments newTender = entity.Entity;
        try
        {
            bool success = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
            if (success)
            {
                await _cacheService.RemoveCacheItemAsync(CacheKeys.TenderDocuments);
                _ = await UpdateCacheTenderDocumentsAsync();
                return newTender;
            }
        }
        catch (Exception)
        {
            throw;
        }
        return newTender;
    }
    public async Task<bool> DeleteTendersDocumentsAsync(TenderDocuments tenderDocuments)
    {

        _ = _context.TenderDocuments.Remove(tenderDocuments);
        bool success = await _context.SaveChangesAsync() > 0;
        if (success)
        {
            await _cacheService.RemoveCacheItemAsync(CacheKeys.TenderDocuments);
        }
        return success;
    }
    private async Task<bool> UpdateCacheTenderDocumentsAsync()
    {

        IEnumerable<TenderDocuments> allTenderDocuments = await _context.TenderDocuments.ToArrayAsync().ConfigureAwait(false);
        await _cacheService.SetAsync(CacheKeys.TenderDocuments, allTenderDocuments).ConfigureAwait(false);
        return true;
    }
}
