using Core.Entities.Vouchers;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Services;

public class StationActivityHoursTemplatesService : IStationActivityHoursTemplatesService
{
    private readonly RailDbContext _context;
    private readonly ICacheService _cacheService;

    public StationActivityHoursTemplatesService(RailDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }



    public async Task<IEnumerable<StationActivityHoursTemplateLinesDto>> GetStationActivityHoursTemplateLinesAsync()
    {
        return await GetStationActivityHoursTemplateLinesNoCache().ConfigureAwait(false);
    }
    public async Task<IEnumerable<StationActivityTemplatesTypes>> GetStationActivityTemplatesTypesAsync()
    {
        return await GetStationActivityTemplatesTypesNoCache().ConfigureAwait(false);
    }
    public async Task<IEnumerable<TemplatesDto>> GetTamplatesByType(int tamplateTypeId)
    {
        IEnumerable<StationActivityHoursTemplateLinesDto> Templates = await GetStationActivityHoursTemplateLinesNoCache().ConfigureAwait(false);
        var t = Templates.Where(x => x.TemplateTypeId == tamplateTypeId).Select(x =>
                                new TemplatesDto
                                {
                                    TemplateTypeId = x.TemplateTypeId,
                                    TemplateId = x.TemplateId,
                                    TemplateName = x.TemplateName

                                }).ToArray();
        var x = t.GroupBy(x => x.TemplateId).Select(grp => grp.First()).ToArray();
        return x;

    }
    private async Task<IEnumerable<StationActivityHoursTemplateLinesDto>> GetStationActivityHoursTemplateLinesNoCache()
    {
        IEnumerable<StationActivityHoursTemplateLinesDto> stationActivityHoursTemplatesLines  = await _context.StationActivityHoursTemplates
                                                                                     .Join(_context.StationActivityHoursTemplatesLine,
                                                            x => x.TemplateId,
                                                            y => y.TemplateId,
                                                            (x, y) =>


                                                            new StationActivityHoursTemplateLinesDto
                                                            {
                                                                TemplateId = x.TemplateId,
                                                                ActivityDaysNumbers = y.ActivityDaysNumbers,
                                                                EndHour = y.EndHour,
                                                                StartHour = y.StartHour,
                                                                TemplateLineId = y.Id,
                                                                TemplateName = x.TemplateName,
                                                                TemplateTypeId = x.TemplateTypeId,
                                                                ActivityHoursReplaceTextKey = y.ActivityHoursReplaceTextKey,
                                                                EndHourPostfixTextKey = y.EndHourPostfixTextKey,
                                                                EndHourPrefixTextKey = y.EndHourPostfixTextKey,
                                                                EndHourReplaceTextKey = y.EndHourReplaceTextKey,
                                                                StartHourPrefixTextKey = y.StartHourPrefixTextKey,
                                                                StartHourReplaceTextKey = y.StartHourReplaceTextKey
                                                            })

                                                         .ToArrayAsync().ConfigureAwait(false);
               return stationActivityHoursTemplatesLines;
    }

    public async Task<IEnumerable<StationActivityTemplatesTypes>> GetStationActivityTemplatesTypesNoCache()
    {
        return await _context.StationActivityTemplatesTypes
                                                     .ToArrayAsync().ConfigureAwait(false);
    }
  
    #region AddDeleteUpdate
    public async Task<StationActivityHoursTemplates> AddTemplateAsync(StationActivityHoursTemplates stationActivityHoursTemplates, IEnumerable<StationActivityHoursTemplatesLine> stationActivityHoursTemplatesLines)
    {
        StationActivityHoursTemplates obj = null;
        EntityEntry<StationActivityHoursTemplates> entity = await _context.StationActivityHoursTemplates.AddAsync(stationActivityHoursTemplates).ConfigureAwait(false);

        if (await _context.SaveChangesAsync().ConfigureAwait(false) > 0)
        {
            obj = entity.Entity;

            await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.StationActivityHoursTemplates);
        }

        foreach (var item in stationActivityHoursTemplatesLines)
        {
            item.TemplateId = entity.Entity.TemplateId;
            _ = _context.StationActivityHoursTemplatesLine.Add(item);
        }
        _ = await _context.SaveChangesAsync().ConfigureAwait(false);
        IEnumerable<StationActivityHoursTemplateLinesDto> allstationActivityHoursTemplatesLines = await GetStationActivityHoursTemplateLinesNoCache().ConfigureAwait(false);
        await _cacheService.SetAsync<IEnumerable<StationActivityHoursTemplateLinesDto>>(CacheKeys.StationActivityHoursTemplates, allstationActivityHoursTemplatesLines).ConfigureAwait(false);
        return obj;
    }
    public async Task<bool> DeleteTemplateAsync(int Id)
    {
        StationActivityHoursTemplates template = await _context.StationActivityHoursTemplates.SingleOrDefaultAsync(p => p.TemplateId == Id).ConfigureAwait(false);
        bool success = false;
        if (template != null)
        {
            _ = _context.StationActivityHoursTemplates.Remove(template);
            success = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
            if (success)
            {
                await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.StationActivityHoursTemplates);
            }
        }
        IEnumerable<StationActivityHoursTemplateLinesDto> allstationActivityHoursTemplatesLines = await GetStationActivityHoursTemplateLinesNoCache().ConfigureAwait(false);
        await _cacheService.SetAsync<IEnumerable<StationActivityHoursTemplateLinesDto>>(CacheKeys.StationActivityHoursTemplates, allstationActivityHoursTemplatesLines).ConfigureAwait(false);
        return success;
    }
    public async Task<bool> UpdateTemplateAsync(StationActivityHoursTemplates stationActivityHoursTemplates, IEnumerable<StationActivityHoursTemplatesLine> stationActivityHoursTemplatesLines, List<int> deletedIds)
    {
        StationActivityHoursTemplates obj = null;


        EntityEntry<StationActivityHoursTemplates> entity = _context.StationActivityHoursTemplates.Update(stationActivityHoursTemplates);

        if (await _context.SaveChangesAsync().ConfigureAwait(false) > 0)
        {
            obj = entity.Entity;

            await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.StationActivityHoursTemplates);
        }



        foreach (var item in stationActivityHoursTemplatesLines)
        {

            if (item.GetType().GetProperty("Id") != null)
            {
                EntityEntry<StationActivityHoursTemplatesLine> entityTamplateLine = _context.StationActivityHoursTemplatesLine.Update(item);
            }
            else
            {
                EntityEntry<StationActivityHoursTemplatesLine> entityTamplateLine = _context.StationActivityHoursTemplatesLine.Add(item);
            }
        }

        foreach (var id in deletedIds)
        {
            StationActivityHoursTemplatesLine line = await _context.StationActivityHoursTemplatesLine.SingleOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
                       EntityEntry<StationActivityHoursTemplatesLine> entityTamplateLine = _context.StationActivityHoursTemplatesLine.Remove(line);
        }

        IEnumerable<StationActivityHoursTemplateLinesDto> allstationActivityHoursTemplatesLines = await GetStationActivityHoursTemplateLinesNoCache().ConfigureAwait(false);
        await _cacheService.SetAsync<IEnumerable<StationActivityHoursTemplateLinesDto>>(CacheKeys.StationActivityHoursTemplates, allstationActivityHoursTemplatesLines).ConfigureAwait(false);

        bool result = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;

        return result;
    }
    #endregion AddDeleteUpdate
}
