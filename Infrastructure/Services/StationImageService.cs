using Core.Entities.Stations;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Infrastructure.Services;

public class StationImageService : IStationImage
{
    private readonly RailDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly IStationsService _stationsService;
    private readonly SpecialDbContext _specialDbContext;
    private readonly ILogger<MessengerService> _logger;
    public StationImageService(RailDbContext context, ICacheService cacheService, ILogger<MessengerService> logger, IStationsService stationsService, SpecialDbContext specialDbContext)
    {
        _context = context;
        _cacheService = cacheService;
        _logger = logger;
        _stationsService = stationsService;
        _specialDbContext = specialDbContext;
    }

    public async Task<IEnumerable<StationImage>> GetAllSvgElementsAsync()
    {
        try
        {
            IEnumerable<StationImage> res = await GetStationsImageContentAsync();
            return res.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError("Get SVG elements error occured ", ex);
            throw;
        }
    }
                public async Task<bool> CheckForUpdate()
    {
        List<bool> needUpdate = new();
        DateTime current = DateTime.UtcNow;
        IEnumerable<StationImage> allImageElements = await GetStationsImageContentAsync();

        IEnumerable<StationImage> inBetweenElements = allImageElements.Where(p => p.ToDate != null)
            .Where(x => x.LastUpdated == null && x.FromDate < current && (DateTime)x.ToDate > current);

        IEnumerable<StationImage> overElements = allImageElements.Where(p => p.ToDate != null)
            .Where(x => x.LastUpdated != null && x.FromDate < current && (DateTime)x.ToDate < current);

              

        if (inBetweenElements.Count() > 0)
        {
            foreach (StationImage element in inBetweenElements)
            {
                element.LastUpdated = current;
                if (element.BetweenDatesInactive)
                {
                    needUpdate.Add(await UpdateElementActivationAsync(element, true, current, element.BetweenDatesInactive, (DateTime)element.ToDate));
                }

                if (!element.BetweenDatesInactive)
                {
                    needUpdate.Add(await UpdateElementActivationAsync(element, false, current, element.BetweenDatesInactive, (DateTime)element.ToDate));
                }
            }
        }

        if (overElements.Count() > 0)
        {
            foreach (StationImage element in overElements)
            {
                element.LastUpdated = null;
                element.BetweenDatesInactive = false;
                if (element.BetweenDatesInactive)
                {
                    needUpdate.Add(await UpdateElementActivationAsync(element, true, current, element.BetweenDatesInactive, (DateTime)element.ToDate));
                }

                if (!element.BetweenDatesInactive)
                {
                    needUpdate.Add(await UpdateElementActivationAsync(element, false, current, element.BetweenDatesInactive, (DateTime)element.ToDate));
                }
            }
        }

        if (needUpdate.Any(x => x == true))
        {
            _ = await BuildSVGAsync();
            return true;
        }
        else
        {
            return false;
        }
    }
                public async Task<string> GetStationSVGAsync()
    {
        try
        {

            bool g = await CheckForUpdate();
            IEnumerable<StationImageComplete> res = await GetStationsImageCompleteContentAsync();
            return JsonConvert.SerializeObject(res.First().Code);
        }
        catch (Exception ex)
        {
            _logger.LogError("Get SVG error occured ", ex);
            throw;
        }
    }

    public async Task<object> GetStationSVGPerLanguageAsync()
    {
        try
        {

            bool g = await CheckForUpdate();
            IEnumerable<StationImageComplete> res = await GetStationsImageCompleteContentAsync();
            return res.First();
        }
        catch (Exception ex)
        {
            _logger.LogError("Get SVG error occured ", ex);
            throw;
        }
    }
                                                                                                            
                                                      
   
                     
                                  public async Task<bool> UpdateElementActivationAsync(StationImage element, bool closed, DateTime fromdate, bool betweenDatesInactive, DateTime? todate = null)
    {
        bool success = false;
                             element.BetweenDatesInactive = betweenDatesInactive;
            element.FromDate = fromdate.ToUniversalTime();
            element.ToDate = !(todate == null) ? ((DateTime)todate).ToUniversalTime() : null;
            element.IsInactive = closed;
            
            XElement xmlTree = XElement.Parse(element.ElementCode);
           


            if (xmlTree.Attribute("id").Value.Contains(element.ElementKey) && element.ElementKey.Length < 5)
            {
                foreach (XElement attr in xmlTree.Descendants().Where(p => p.Name == "circle" || p.Name == "path").Select(x => x))
                {
                    if (attr.Attribute("class") != null)
                    {
                        if (closed)
                        {
                            attr.Attribute("class").SetValue("inactivestation");
                            element.IsInactive = true;
                        }
                        else
                        {
                            attr.Attribute("class").SetValue("activestation");
                            element.IsInactive = false;
                        }
                    }
                }
            }

            if (xmlTree.Attribute("id").Value.Contains(element.ElementKey) && element.ElementKey.Length > 5)
            {
                if (closed)
                {
                    xmlTree.Attribute("class").SetValue(xmlTree.Attribute("class").Value + " inactiveline");
                    element.IsInactive = true;
                }
                else
                {
                                                              xmlTree.Attribute("class").SetValue(xmlTree.Attribute("class").Value.Replace(" inactiveline", "")); 
                                   element.IsInactive = false;
                }
            }
            element.ElementCode = xmlTree.ToString();
            #region
                                                                                                                                                                                                                                                                                    #endregion
                      EntityEntry<StationImage> entity = _context.StationImage.Update(element);
            _context.Entry(element).State = EntityState.Modified;
          
                     success = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
            if (success)
            {
                await _cacheService.RemoveCacheItemAsync(CacheKeys.StationImage);
                var r=await _context.StationImage.ToArrayAsync().ConfigureAwait(false);
                await _cacheService.SetAsync(CacheKeys.StationImage, r).ConfigureAwait(false);
            }
           
        return success;
    }
    public   async Task<IEnumerable<StationImage>> GetStationsImageContentAsync()
    {
        IEnumerable<StationImage> allImageElements = await _cacheService.GetAsync<IEnumerable<StationImage>>(CacheKeys.StationImage).ConfigureAwait(false);
        if (allImageElements == null || allImageElements.Count() <= 0)
        {
            allImageElements = await _context.StationImage.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync(CacheKeys.StationImage, allImageElements).ConfigureAwait(false);
                                            
        }

        return allImageElements;
    }
    private async Task<IEnumerable<StationImageComplete>> GetStationsImageCompleteContentAsync()
    {
        IEnumerable<StationImageComplete> stationImageComplete = await _cacheService.GetAsync<IEnumerable<StationImageComplete>>(CacheKeys.StationImageComplete).ConfigureAwait(false);
        if (stationImageComplete == null || stationImageComplete.Count() <= 0)
        {
            stationImageComplete = await _context.StationImageComplete.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync(CacheKeys.StationImageComplete, stationImageComplete).ConfigureAwait(false);
        }
        return stationImageComplete;
    }
                   public async Task<string> BuildSVGAsync()//BuildSVGPerLanguageAsync()
    {
               IEnumerable<StationImage> allImageElements  = await _specialDbContext.StationImageNames.FromSqlRaw("exec [dbo].[UpdateStationImageNames]").ToArrayAsync().ConfigureAwait(false);
       
        Assembly asm = Assembly.GetExecutingAssembly();
        string resourceName = "Infrastructure.Templates.StationsImageTemplate.svg";
        string[] resourceNameLang = { "Infrastructure.Templates.StationsImageTemplateAR.svg",
                                        "Infrastructure.Templates.StationsImageTemplateHE.svg",
                                        "Infrastructure.Templates.StationsImageTemplateEN.svg",
                                        "Infrastructure.Templates.StationsImageTemplateRU.svg" };
               XmlDocument xmlDocument = new();
        List<XmlDocument> xmlDocumentLang= new();
                  List<StationImage> stationsImage = allImageElements.Where(p => p.ElementKey.Length < 5).Select(x => x).ToList();
        List<StationImage> linesImage = allImageElements.Where(p => p.ElementKey.Length > 5).Select(x => x).ToList();
        IEnumerable<StationImageComplete> stationImageComplete = await _context.StationImageComplete.ToArrayAsync().ConfigureAwait(false);
        bool success = false;
        try
        {
            using Stream stream = asm.GetManifestResourceStream(resourceName);
            using XmlTextReader reader = new(stream);
            xmlDocument.Load(reader);
            List<string> xmlTemplateLang = new();
            for (int i=0;i< resourceNameLang.Length;i++)
            {
                using Stream streamLang = asm.GetManifestResourceStream(resourceNameLang[i]);
                using XmlTextReader readerLang = new(streamLang);
                xmlDocumentLang.Add(new());
                xmlDocumentLang[i].Load(readerLang);
                xmlTemplateLang.Add(xmlDocumentLang[i].InnerXml);
                streamLang.Close();
            }
            string xmlTemplate = xmlDocument.InnerXml;
            string si = string.Empty;
            List<string> siLang = new();
            foreach (StationImage item in stationsImage)
            {
                if (siLang.Count == 0)
                {
                    siLang.Add("");
                    siLang.Add("");
                    siLang.Add("");
                    siLang.Add("");
                }

                si += item.ElementCode;// : "";
                siLang[0] += item.ElementCodeAR;
                siLang[1] += item.ElementCodeHE;
                siLang[2] += item.ElementCodeEN;
                siLang[3] += item.ElementCodeRU;
            }

            xmlTemplate = xmlTemplate.Replace("<!--Stations-->", "<!--Stations-->" + si);
            si = "";
            foreach (StationImage item in linesImage)
            {
                si += item.ElementCode;// : "";
            }
            xmlTemplate = xmlTemplate.Replace("<!--Train track lines-->", "<!--Train track lines-->" + si);
            for (int i = 0; i < xmlTemplateLang.Count; i++)
            {
                xmlTemplateLang[i] = xmlTemplateLang[i].Replace("<!--Stations-->", "<!--Stations-->" + siLang[i]);
                xmlTemplateLang[i] = xmlTemplateLang[i].Replace("<!--Train track lines-->", "<!--Train track lines-->" + si);
            }
            StationImageComplete stationImageCompleteNew = new();
            if (stationImageComplete.Count() < 1)
            {
                stationImageCompleteNew.Code = xmlTemplate;
                stationImageCompleteNew.CodeAR = xmlTemplateLang[0];
                stationImageCompleteNew.CodeHe = xmlTemplateLang[1];
                stationImageCompleteNew.CodeEn = xmlTemplateLang[2];
                stationImageCompleteNew.CodeRu = xmlTemplateLang[3];
                _ = _context.StationImageComplete.Attach(stationImageCompleteNew);
            }
            else
            {
                stationImageCompleteNew.Code = xmlTemplate;
                stationImageCompleteNew.CodeAR = xmlTemplateLang[0];
                stationImageCompleteNew.CodeHe = xmlTemplateLang[1];
                stationImageCompleteNew.CodeEn = xmlTemplateLang[2];
                stationImageCompleteNew.CodeRu = xmlTemplateLang[3];
                _ = _context.StationImageComplete.Remove(stationImageComplete.FirstOrDefault());
                _ = _context.StationImageComplete.Attach(stationImageCompleteNew);
            }
            _context.Entry(stationImageCompleteNew).State = EntityState.Added;

            success = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
            if (success)
            {

                await _cacheService.RemoveCacheItemAsync(CacheKeys.StationImage);
                await _cacheService.RemoveCacheItemAsync(CacheKeys.StationImageComplete);
                                                 
           
                
            }

            stream.Close();

            return JsonConvert.SerializeObject("SVG created");
        }
        catch (Exception ex)
        {
            _logger.LogError("SVG creation error occured ", ex);
            throw;
        }
    }
}
