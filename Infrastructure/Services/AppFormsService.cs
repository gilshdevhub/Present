using Core.Entities.Forms;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Infrastructure.Services;


public class AppFormsService : IAppFormsService
{
    private readonly ICacheService _cacheService;
    private readonly RailDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AppFormsService> _logger;
    public readonly Dictionary<string, string> map = new(StringComparer.OrdinalIgnoreCase)
    {
        [".png"] = "image/png",
        [".jpg"] = "image/jpeg",
        [".jpeg"] = "image/jpeg",
        [".gif"] = "image/gif",
        [".doc"] = "application/msword",
        [".docx"] = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        [".pdf"] = "application/pdf",
        [".txt"] = "text/*",
        [".xls"] = "application/vnd.ms-excel",
        [".xlsx"] = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
    };
    public readonly HttpClient _httpClient = new();
    public AppFormsService(RailDbContext context, IConfiguration configuration, ILogger<AppFormsService> logger, ICacheService cacheService)
    {
        _configuration = configuration;
        _logger = logger;
        _context = context;
        _cacheService = cacheService;
    }
    public Task<FormsResponse> CrmFormResult(string data)
    {
        System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
        xd.LoadXml(data.Trim());
        FormsResponse formsResponse = new FormsResponse();
        formsResponse.code= xd["html"]["body"]["form"]["div"].ChildNodes[2].InnerText.Trim();
        formsResponse.guid=xd["html"]["body"]["form"]["div"].ChildNodes[1].InnerText.Trim();
        if (string.IsNullOrEmpty(formsResponse.code)|| string.IsNullOrEmpty(formsResponse.guid))
        {
            formsResponse.code = ((int)HttpStatusCode.NoContent).ToString();
            formsResponse.guid = "CRM error.";
        }
        return Task.FromResult(formsResponse);
    }

       public async Task<FormsResponse> PostFormsAsync(FullForms fullForms)
    {
        string urlDestination = _configuration.GetSection("CRMdestination").Value;
        IFormFile[] files = fullForms.files;
        int i = 0;
        string data;
        var formThree = await GetThreeByIdAsync(fullForms.formId);
        using (var multipart = new MultipartFormDataContent())
        {
            multipart.Add(new StringContent(System.Text.Json.JsonSerializer.Serialize(fullForms)));
            #region old
                                                                                                                                                                                           
                                                                                                                                                                                                                                                                                                                                          
                                  #endregion
            multipart.Add(new StringContent(_configuration.GetSection("CRMToken").Value), "token");

            foreach (var formFile in files)
            {
                if (i < 5)
                {
                    if (formFile.Length > 0)
                    {
                        using var memoryStream = new MemoryStream();
                        await formFile.CopyToAsync(memoryStream);
                        string ext = Path.GetExtension(formFile.FileName);
                                               if (memoryStream.Length < 10485760)
                        {
                            if (map.TryGetValue(ext, out string contentType))
                            {
                                var file = new upFile()
                                {
                                    Content = memoryStream.ToArray()
                                };

                                var fileStreamContent = new ByteArrayContent(file.Content);
                                fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
                                multipart.Add(fileStreamContent, name: $"file{i + 1}", fileName: formFile.FileName);
                            }

                        }
                        else
                        {
                            _logger.LogInformation("The file is too large.", formFile);
                            return  new FormsResponse { guid = "The file is too large.", code = ((int)HttpStatusCode.RequestEntityTooLarge).ToString() };
                        }
                    }
                    i++;
                }
                if(i>5)
                    return new FormsResponse { guid = "Only 5 files are allowed.",code= ((int)HttpStatusCode.RequestEntityTooLarge).ToString() };
            }
            try
            {
                using HttpResponseMessage response = await _httpClient.PostAsync(urlDestination, multipart);
                _ = response.EnsureSuccessStatusCode();
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error sending form", ex);
                return new FormsResponse { guid = ex.Message,code= ((int)HttpStatusCode.InternalServerError).ToString() };
            }
        }
                      FormsResponse x = await CrmFormResult(data);
        return x;

    }

    public async Task<string> PostFormsAspxAsync(FullForms fullForms)
    {
        string urlDestination = _configuration.GetSection("CRMdestination").Value;
        IFormFile[] files = fullForms.files;
        int i = 0;
        string data;
        var formThree = await GetThreeByIdAsync(fullForms.formId);
        using (var multipart = new MultipartFormDataContent())
        {
            multipart.Add(new StringContent(System.Text.Json.JsonSerializer.Serialize(fullForms)));
            multipart.Add(new StringContent(_configuration.GetSection("CRMToken").Value), "token");

            foreach (var formFile in files)
            {
                if (i < 5)
                {
                    if (formFile.Length > 0)
                    {
                        using var memoryStream = new MemoryStream();
                        await formFile.CopyToAsync(memoryStream);
                        string ext = Path.GetExtension(formFile.FileName);
                                               if (memoryStream.Length < 10485760)
                        {
                            if (map.TryGetValue(ext, out string contentType))
                            {
                                var file = new upFile()
                                {
                                    Content = memoryStream.ToArray()
                                };

                                var fileStreamContent = new ByteArrayContent(file.Content);
                                fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
                                multipart.Add(fileStreamContent, name: $"file{i + 1}", fileName: formFile.FileName);
                            }

                        }
                        else
                        {
                            _logger.LogInformation("The file is too large.", formFile);
                            return "The file is too large.";//, code = ((int)HttpStatusCode.RequestEntityTooLarge).ToString() };
                        }
                    }
                    i++;
                }
                if (i > 5)
                    return "Only 5 files are allowed.";//, code = ((int)HttpStatusCode.RequestEntityTooLarge).ToString() };
            }
            try
            {
                using HttpResponseMessage response = await _httpClient.PostAsync(urlDestination, multipart);
                _ = response.EnsureSuccessStatusCode();
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error sending form", ex);
                return ex.Message;//, code = ((int)HttpStatusCode.InternalServerError).ToString() };
            }
        }
                             return data;

    }
    public async Task<FormsIdThrees> GetThreeByIdAsync(int formId)
    {
        IEnumerable<FormsIdThrees> formsIdThrees = await _cacheService.GetAsync<IEnumerable<FormsIdThrees>>(CacheKeys.FormsIdThrees).ConfigureAwait(false);
        if (formsIdThrees == null || formsIdThrees.Count() <= 0)
        {
            formsIdThrees = await _context.FormsIdThrees.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<FormsIdThrees>>(CacheKeys.FormsIdThrees, formsIdThrees).ConfigureAwait(false);
        }

        FormsIdThrees FormResult = formsIdThrees.FirstOrDefault(x=>x.formId== formId); 
        return FormResult;
    }
    public async Task<IEnumerable<FormsIdThrees>> GetThreesAsync()
    {
        IEnumerable<FormsIdThrees> formsIdThrees = await _cacheService.GetAsync<IEnumerable<FormsIdThrees>>(CacheKeys.FormsIdThrees).ConfigureAwait(false);
        if (formsIdThrees == null || formsIdThrees.Count() <= 0)
        {
            formsIdThrees = await _context.FormsIdThrees.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<FormsIdThrees>>(CacheKeys.FormsIdThrees, formsIdThrees).ConfigureAwait(false);
        }
        return formsIdThrees;
    }

    public async Task<FormsIdThrees> AddFormsIdThreeAsync(FormsIdThrees formsIdThreeToAdd)
    {
        FormsIdThrees formsIdThree = null;

        EntityEntry<FormsIdThrees> entity = await _context.FormsIdThrees.AddAsync(formsIdThreeToAdd).ConfigureAwait(false);

        if (await _context.SaveChangesAsync().ConfigureAwait(false) > 0)
        {
            formsIdThree = entity.Entity;
            await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.FormsIdThrees);
        }

        return formsIdThree;
    }

    public async Task<bool> UpdateFormsIdThreesAsync(FormsIdThrees formsIdThreeToUpdate)
    {
        _ = _context.FormsIdThrees.Update(formsIdThreeToUpdate);

        if (await _context.SaveChangesAsync().ConfigureAwait(false) > 0)
        {
            IEnumerable<FormsIdThrees> allForms = await _context.FormsIdThrees.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.RemoveCacheItemAsync(CacheKeys.FormsIdThrees);
            await _cacheService.SetAsync(CacheKeys.FormsIdThrees, allForms).ConfigureAwait(false);
          
        }

        return true;
    }

    public async Task<bool> DeleteFormsIdThreeAsync(int formId)
    {
        FormsIdThrees formsIdThree = await _context.FormsIdThrees.SingleOrDefaultAsync(p => p.formId == formId).ConfigureAwait(false);
        bool success = false;
        if (formsIdThree != null)
        {
            _ = _context.FormsIdThrees.Remove(formsIdThree);
            success = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
            if (success)
            {
                await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.FormsIdThrees);
            }
        }
        return success;
    }
   }
