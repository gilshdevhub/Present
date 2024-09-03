using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces;

public interface IEligibilityService
{
    Task<bool> UploadFileAsync(IFormFile FileToLoad);
    Task<RavKav> Checking(string ravKavNumber);
}
