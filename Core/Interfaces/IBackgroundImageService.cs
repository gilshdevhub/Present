using Core.Entities;
using Core.Entities.Push;
using Core.Entities.Translation;
using Core.Entities.Vouchers;

namespace Core.Interfaces;

public interface IBackgroundImageService
{
    Task<bool> CheckDatesAsync(BackgroundImage item);
    Task<bool> UpdateDatesAsync(DateTime from, DateTime untill,string Name);
    Task<bool> DeleteAsync(string item);
    Task UploadBackGroundAsync(UploadBackGround uploadFile);
    Task<Dictionary<string, BGResponse>> DownloadBackgroundFilesAsync();
    Task<List<BackgroundImageDto>> DownloadAllBackgroundFilesAsync();
}
