using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Core.Entities;
using Core.Entities.Push;
using Core.Entities.Translation;
using Core.Entities.Vouchers;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using System.Drawing;
using System.Text.Json;

namespace Infrastructure.Services;

public class BackgroundImageService : IBackgroundImageService
{
    private readonly RailDbContext _context;
    private readonly BlobServiceClient _blobServiceClient;

    public BackgroundImageService(RailDbContext context, BlobServiceClient blobServiceClient)
    {
        _context = context;
        _blobServiceClient = blobServiceClient;
    }

    public readonly Dictionary<string, string> mapBackground = new(StringComparer.OrdinalIgnoreCase)
    {
        [".jpeg"] = "image/jpeg",
        [".jpg"] = "image/jpeg",
        [".png"] = "image/png"
    };

    public async Task<bool> CheckDatesAsync(BackgroundImage item)
    {

        if (item != null)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
                       if (item.IsTempExists == true)
            {
                if (dateTime <= item.Untill.Date)
                {
                    return true;
                }
                else
                {
                    await DeleteAsync(item.Name);
                }
            }

        }
        return false;
    }
    public async Task<bool> UpdateDatesAsync(DateTime from, DateTime untill, string name)
    {
        try
        {
            var item = await _context.BackgroundImage.Where(x => x.Name == name).SingleOrDefaultAsync().ConfigureAwait(false);

            if (item == null)
            {
                var itemNew = new BackgroundImage();
                itemNew.From = from;
                itemNew.Untill = untill;
                item.IsTempExists = true;
                _ = _context.BackgroundImage.Add(itemNew);
            }
            else
            {
                item.Untill = untill;
                item.From = from;
                item.IsTempExists = true;
                _ = _context.BackgroundImage.Attach(item);
                _context.Entry(item).State = EntityState.Modified;
            }

            bool success = await _context.SaveChangesAsync() > 0;
            return success;
        }
        catch (Exception ex)
        {

            string s = ex.Message.ToString();
            return false;
        }
    }
        
    public async Task<bool> DeleteAsync(string name)
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient("background");
        var blob = blobContainer.GetBlobClient($"{name}.jpg");

        var item = await _context.BackgroundImage.Where(x => x.Name == name).FirstOrDefaultAsync();

        item.IsTempExists = false;
        _ = _context.BackgroundImage.Attach(item);
        _context.Entry(item).State = EntityState.Modified;
        bool success = await _context.SaveChangesAsync() > 0;
        await blob.DeleteIfExistsAsync();
        return true;

    }
    public async Task<Dictionary<string, BGResponse>> DownloadBackgroundFilesAsync()
    {
        var BackgroundImageTableData = await _context.BackgroundImage.ToListAsync();
        var blobContainer = _blobServiceClient.GetBlobContainerClient("background");
        if (!blobContainer.Exists())
        {
            return null;
        }

        Dictionary<string, BGResponse> result = new Dictionary<string, BGResponse>();
        foreach (var backgroundImage in BackgroundImageTableData)
        {
            if (await CheckDatesAsync(backgroundImage))
            {
                string ImageName = $"{backgroundImage.Name}";
                var blobClient = blobContainer.GetBlobClient($"{ImageName}.jpg");
                if (await blobClient.ExistsAsync())
                {
                    BGResponse res = new BGResponse();
                    res.file = await GetFileByDetails(blobContainer, ImageName, blobClient);
                    res.Start = backgroundImage.From;
                    res.End = backgroundImage.Untill;
                    result.Add(ImageName, res);
                }
            }
        }

        return result;


    }
    private async Task<byte[]> GetFileByDetails(BlobContainerClient blobContainer, string ImageName, BlobClient blobClient)
    {
        var downloadStream = new MemoryStream();
        try
        {
            var cancellationTokenSource = new CancellationTokenSource(30000);
            var cancellationToken = cancellationTokenSource.Token;

            await blobClient.DownloadToAsync(downloadStream, cancellationToken: cancellationToken);
            downloadStream.Position = 0;
            return downloadStream.ToArray();
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public async Task<List<BackgroundImageDto>> DownloadAllBackgroundFilesAsync()
    {
               var backgroundImageTableData = await _context.BackgroundImage.ToListAsync();
               List<BackgroundImageDto> dataRes = new List<BackgroundImageDto>();
        backgroundImageTableData.ForEach(item =>
        {
            dataRes.Add(new BackgroundImageDto
            {
                Decription = item.Decription,
                From = item.From.Date,
                Height = item.Height,
                Id = item.Id,
                Name = item.Name,
                Untill = item.Untill.Date,
                Weight = item.Weight,
                Width = item.Width,
                IsTempExists = item.IsTempExists
            });

        });
        var blobContainer = _blobServiceClient.GetBlobContainerClient("background");
        if (!blobContainer.Exists())
        {
            return null;
        }



        foreach (BackgroundImageDto backgroundImage in dataRes)
        {
            Dictionary<string, byte[]> dic = new Dictionary<string, byte[]>();
            string ImageName = backgroundImage.Name;
            string ImageNameDefault = $"{backgroundImage.Name}_default";
            var blobClient = blobContainer.GetBlobClient($"{ImageName}.jpg");
            if (await blobClient.ExistsAsync())
            {
                dic.Add("temp", await GetFileByDetails(blobContainer, ImageName, blobClient));
            }


            blobClient = blobContainer.GetBlobClient($"{ImageNameDefault}.jpg");
            if (await blobClient.ExistsAsync())
            {
                dic.Add("default", await GetFileByDetails(blobContainer, ImageNameDefault, blobClient));
            }
            backgroundImage.images = dic;        }

        return dataRes;

    }

    public async Task UploadBackGroundAsync(UploadBackGround uploadFile)
    {
        IFormFile file = uploadFile.FileToLoad;
        var blobContainer = _blobServiceClient.GetBlobContainerClient("background");
        if (!blobContainer.Exists())
        {
            _ = blobContainer.Create();
        }


        string pathToFile = $"{uploadFile.Name}.jpg";


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
        if (mapBackground.TryGetValue(ext, out string contentType))
        {
            options.HttpHeaders = new BlobHttpHeaders { ContentType = contentType };
            _ = await blobClient.UploadAsync(fs, options);
        }
    }
}
