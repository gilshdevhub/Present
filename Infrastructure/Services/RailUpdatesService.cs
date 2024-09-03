using Aspose.Svg;
using Core.Config;
using Core.Entities.Configuration;
using Core.Entities.RailUpdates;
using Core.Entities.Translation;
using Core.Enums;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Umbraco.Headless.Client.Net.Delivery;

namespace Infrastructure.Services;

public class RailUpdatesService : IRailUpdatesService
{
    private readonly RailUpdatesConfig _railUpdatesConfig;
    private readonly IHttpClientService _httpClient;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    public string umbracoKey = string.Empty;
    private readonly ICacheService _cacheService;
    private readonly IConfigurationService _configurationService;
    public RailUpdatesService(IOptions<RailUpdatesConfig> railUpdatesConfig, IHttpClientService httpClient, IConfiguration configuration,
        IHttpClientFactory httpClientFactory, ICacheService cacheService, IConfigurationService configurationService)
    {
        _railUpdatesConfig = railUpdatesConfig.Value;
        _httpClient = httpClient;
        _configuration = configuration;
        umbracoKey = _configuration.GetSection("UmbracoKey").Value;
        _httpClientFactory = httpClientFactory;
        _cacheService = cacheService;
        _configurationService = configurationService;
    }

    public async Task<RailUpdate> GetRailGeneralUpdatesAsync(Languages languageId)
    {
        string json = await _httpClient.GetRailInfoAsync(_railUpdatesConfig.General.ApiUrl, _railUpdatesConfig.General.ContentType).ConfigureAwait(false);

        RailUpdate railUpdates = JsonConvert.DeserializeObject<RailUpdate>(json);

        railUpdates.Data = railUpdates.Filter(languageId);
        railUpdates.Data = railUpdates.Data.Where(p => DateTime.Now >= p.StartValidationOfReport && DateTime.Now <= p.EndValidationOfReport).ToArray();
        return railUpdates;
    }

    public async Task<IEnumerable<RailUpdateResponseUmbracoDto>> GetRailGeneralUpdatesUmbracoAsync(Languages languageId, SystemTypes systemType)
    {
        List<RailUpdateResponseUmbracoDto> railUpdates = new();
        ContentDeliveryService m = new(_railUpdatesConfig.Umbraco.UmbracoUrl, umbracoKey);
        string umbracoCulture = string.Empty;
        Umbraco.Headless.Client.Net.Delivery.Models.Content content = new();
        switch (languageId)
        {
            case Languages.Hebrew:
                umbracoCulture = "he";
                break;
            case Languages.English:
                umbracoCulture = "en-us";
                break;
            case Languages.Arabic:
                umbracoCulture = "ar";
                break;
            case Languages.Russian:
                umbracoCulture = "ru";
                break;
            default:
                umbracoCulture = "he";
                break;
        }

        try
        {
            content = await m.Content.GetById(new Guid(_railUpdatesConfig.Umbraco.Id), umbracoCulture);
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message, ex);
        }
        var updates = content.Properties["updates"];
        var temp = JsonConvert.DeserializeObject<List<ChildrenDto>>(updates.ToString());
        try
        {
            foreach (var item in temp)
            {

                if (item.endDate > DateTime.Now && item.startDate < DateTime.Now)
                {
                    for (int i = 0; i < item.stations.Count; i++)
                    {
                        int index = item.stations[i].IndexOf("^");
                        if (index >= 0)
                            item.stations[i] = item.stations[i].Substring(index + 1);
                    }
                    string updateLink = _railUpdatesConfig.Umbraco.LinkUrl + JsonConvert.DeserializeObject<List<UpdatePageDto>>(item.updatePage.ToString())[0].id;
                    RailUpdateResponseUmbracoDto railUpdateItem =
                        new(item.header, item.contentdata, updateLink, item.stations, item.linkText, item.updateType);
                    railUpdates.Add(railUpdateItem);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
       
        return railUpdates;

    }

    public async Task<List<RailUpdateResponseUmbracoDto>> GetRailGeneralUpdatesUmbracoByStationIdAsync(Languages languageId, int stationId)
    {
                                                                                                                                                                                                           
                                                                                                                                                                  List<RailUpdateResponseUmbracoDto> railUpdates = await UmbracoGeneralUpdatesByLanguageAsync(languageId, stationId).ConfigureAwait(false);
        return railUpdates;
    }

    public async Task<RailUpdate> GetRailSpecialUpdatesAsync(int originStationId, int targerStationId)
    {
        string apiUrl = string.Format(_railUpdatesConfig.Special.ApiUrl, originStationId, targerStationId);

        string json = await _httpClient.GetRailInfoAsync(apiUrl, _railUpdatesConfig.General.ContentType).ConfigureAwait(false);

        RailUpdate railUpdates = JsonConvert.DeserializeObject<RailUpdate>(json);
        railUpdates.Data = railUpdates.Data.Where(p => DateTime.Now >= p.StartValidationOfReport && DateTime.Now <= p.EndValidationOfReport).ToArray();
        return railUpdates;
    }

    public async Task<IEnumerable<RailUpdateResponseUmbracoDto>> GetRailUpdatesNewAsync(Languages? languageId)
    {
        RailUpdateAllLanguagesDto railUpdates = new();
        try
        {

             railUpdates = await _cacheService.GetAsync<RailUpdateAllLanguagesDto>(CacheKeys.RailUpdatesNew);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }

        if (railUpdates == null)
        {
            await SetRailUpdatesNewAsync().ConfigureAwait(false);
            railUpdates = await _cacheService.GetAsync<RailUpdateAllLanguagesDto>(CacheKeys.RailUpdatesNew);
        
        }
        IEnumerable<RailUpdateResponseUmbracoDto> res ;
        switch (languageId)
        {
            case Languages.Hebrew:
                res = railUpdates.Hebrew;
                break;
            case Languages.English:
                res = railUpdates.English;
                break;
            case Languages.Arabic:
                res = railUpdates.Arabic;
                break;
            case Languages.Russian:
                res = railUpdates.Russian;
                break;
            default:
                res = railUpdates.Hebrew;
                break;
        }
        return res;
    }


    public async Task<bool> SetRailUpdatesWithTimerAsync()
    {
        ConfigurationParameter configuration = await _configurationService.GetItemAsync("timeSpanRailUpdates").ConfigureAwait(false);
        int timeSpanRailUpdates = Int32.Parse(configuration.ValueMob);
        DateTime updateTime = await _cacheService.GetAsync<DateTime>(CacheKeys.RailUpdatesUpdateCacheTime);
        DateTime now = DateTime.Now;
        TimeSpan span = now.Subtract(updateTime);
        if (span.TotalMinutes > timeSpanRailUpdates)
        {
            SetRailUpdatesNewAsync();
            await _cacheService.SetAsync<DateTime>(CacheKeys.RailUpdatesUpdateCacheTime, DateTime.Now).ConfigureAwait(false);
            return true;
        }
        return false;
    }

  
    public async Task SetRailUpdatesNewAsync()
    {
        RailUpdateAllLanguagesDto railUpdateAllLanguagesDto = new();

        railUpdateAllLanguagesDto.Hebrew = await AllUmbracoGeneralUpdatesAsync(Languages.Hebrew).ConfigureAwait(false);
        railUpdateAllLanguagesDto.English = await AllUmbracoGeneralUpdatesAsync(Languages.English).ConfigureAwait(false);
        railUpdateAllLanguagesDto.Arabic = await AllUmbracoGeneralUpdatesAsync(Languages.Arabic).ConfigureAwait(false);
        railUpdateAllLanguagesDto.Russian = await AllUmbracoGeneralUpdatesAsync(Languages.Russian).ConfigureAwait(false);
        await _cacheService.SetAsync<RailUpdateAllLanguagesDto>(CacheKeys.RailUpdatesNew, railUpdateAllLanguagesDto).ConfigureAwait(false);
    }


    public async Task<List<RailUpdateResponseUmbracoDto>> AllUmbracoGeneralUpdatesAsync(Languages? languageId)
    {
        string umbracoCulture;
        switch (languageId)
        {
            case Languages.Hebrew:
                umbracoCulture = "he";
                break;
            case Languages.English:
                umbracoCulture = "en-us";
                break;
            case Languages.Arabic:
                umbracoCulture = "ar";
                break;
            case Languages.Russian:
                umbracoCulture = "ru";
                break;
            default:
                umbracoCulture = "he";
                break;
        }
        try
        {
            string url = String.Format("{0}{1}", _configuration.GetSection("UmbracoInnerContent").Value,
                         String.Format("GetRailGeneralUpdatesFromUmbraco?culture={0}", umbracoCulture));
            List<RailUpdateResponseUmbracoDto> railUpdates = new();
            var json = await _httpClient.GetRailInfoAsync(url, "application/json").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<List<RailUpdateResponseUmbracoDto>>(json);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
                                                            }
    public async Task<List<RailUpdateResponseUmbracoDto>> UmbracoGeneralUpdatesByLanguageAsync(Languages languageId, int stationId)
    {
        string umbracoCulture;
        switch (languageId)
        {
            case Languages.Hebrew:
                umbracoCulture = "he";
                break;
            case Languages.English:
                umbracoCulture = "en-us";
                break;
            case Languages.Arabic:
                umbracoCulture = "ar";
                break;
            case Languages.Russian:
                umbracoCulture = "ru";
                break;
            default:
                umbracoCulture = "he";
                break;
        }
        string url = String.Format("{0}{1}", _configuration.GetSection("UmbracoInnerContent").Value,
                    String.Format("GetRailGeneralUpdatesFromUmbracoByStationId?stationId={0}&culture={1}", stationId, umbracoCulture));
        List<RailUpdateResponseUmbracoDto> railUpdates = new();
        var json = await _httpClient.GetRailInfoAsync(url, "application/json").ConfigureAwait(false);
        var temp = JsonConvert.DeserializeObject<List<RailUpdateResponseFromUmbracoDto>>(json);
        foreach (var item in temp)
        {
            RailUpdateResponseUmbracoDto railUpdateItem =
              new(item.UpdateHeader, item.UpdateContent, item.UpdateLink, item.stations, item.linkText, item.updateType);
            railUpdates.Add(railUpdateItem);
        }
        return railUpdates;
    }
}
