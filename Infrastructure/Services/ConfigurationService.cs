using Core.Entities.Configuration;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Infrastructure.Services;

public class ConfigurationService : IConfigurationService
{
    private readonly RailDbContext _context;
    private readonly ICacheService _cacheService;

    public ConfigurationService(RailDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<ConfigurationParameter>> GetAllItemsAsync()
    {
        IEnumerable<ConfigurationParameter> configurations = await GetCacheConfogurationAsync().ConfigureAwait(false);
        return configurations;
    }
    public async Task<IEnumerable<ConfigurationParameter>> GetAllItemsNoCache()
    {
         return await _context.ConfigurationParameter.ToArrayAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<Configuration>> GetConfigurationsBySystemTypeAsync(SystemTypes SystemTypeId)
    {
        IEnumerable<Configuration> result = Enumerable.Empty<Configuration>();
        IEnumerable<ConfigurationParameter> configurations = await GetCacheConfogurationAsync().ConfigureAwait(false);
        if (SystemTypeId.Equals(SystemTypes.Mobile))
        {
            return result = configurations.Select(x => new Configuration() { Key = x.Key, Value = x.ValueMob, Description = x.Description }).ToArray();
        }
        else if (SystemTypeId.Equals(SystemTypes.Web))
        {
            return result = configurations.Select(x => new Configuration() { Key = x.Key, Value = x.ValueWeb, Description = x.Description }).ToArray();
        }

        return result = configurations.Select(x => new Configuration() { Key = x.Key, Value = x.ValueMob, Description = x.Description }).ToArray();


    }

    /*  public async Task<ConfigurationParameter> GetItemByIdAsync(int id)
      {
          IEnumerable<ConfigurationParameter> configurations = await GetCacheConfogurationAsync().ConfigureAwait(false);
          ConfigurationParameter configuration = configurations.SingleOrDefault(p => p.Id == id);
          return configuration;
      }*/

    public async Task<Configuration> GetItemAsync(string key, SystemTypes systemType)
    {
        Configuration configuration = new();
        IEnumerable<ConfigurationParameter> configurations = await GetCacheConfogurationAsync().ConfigureAwait(false);
        if (systemType.Equals(SystemTypes.Mobile))
        {
            configuration = configurations.Select(x => new Configuration { Key = x.Key, Value = x.ValueMob, Description = x.Description }).SingleOrDefault(p => p.Key == key);
        }
        else if (systemType.Equals(SystemTypes.Web))
        {
            configuration = configurations.Select(x => new Configuration { Key = x.Key, Value = x.ValueWeb, Description = x.Description }).SingleOrDefault(p => p.Key == key);
        }
        else
        {
            configuration = configurations.Select(x => new Configuration { Key = x.Key, Description = x.Description }).SingleOrDefault(p => p.Key == key);
        }

        return configuration;
    }

    public async Task<ConfigurationParameter> GetItemAsync(string key)
    {
        ConfigurationParameter configuration = new();
        IEnumerable<ConfigurationParameter> configurations = await GetCacheConfogurationAsync().ConfigureAwait(false);

        configuration = configurations.Select(x => new ConfigurationParameter { Key = x.Key, Description = x.Description, ValueMob = x.ValueMob, ValueWeb = x.ValueWeb }).SingleOrDefault(p => p.Key == key);


        return configuration;
    }

    
    public async Task<IEnumerable<SystemType>> GetSystemTypesAsync()
    {
        return await _context.SystemTypes.ToArrayAsync();
    }

    private Task<int> CompleteAsync()
    {
        return _context.SaveChangesAsync();
    }

   
    private async Task<IEnumerable<ConfigurationParameter>> GetCacheConfogurationAsync()
    {
        IEnumerable<ConfigurationParameter> configurations = await _cacheService.GetAsync<IEnumerable<ConfigurationParameter>>(CacheKeys.ConfigurationParameter).ConfigureAwait(false);

        if (configurations == null || configurations.Count() <= 0)
        {
            configurations = await _context.ConfigurationParameter.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<ConfigurationParameter>>(CacheKeys.ConfigurationParameter, configurations).ConfigureAwait(false);
        }

        return configurations;
    }
    public async Task<object> GetWeekDaysAsync(DateTime date)
    {
        int day = (int)date.DayOfWeek;
        IEnumerable<ConfigurationParameter> allConfigurations = await _context.ConfigurationParameter.ToArrayAsync().ConfigureAwait(false);// _cacheService.GetAsync<IEnumerable<ConfigurationParameter>>(CacheKeys.ConfigurationParameter).ConfigureAwait(false);
        var result = allConfigurations.Where(x => x.Key == "OccWorkSchedualeForPush").Select(x => x).FirstOrDefault();
        var t = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkHours>(result.ValueMob.ToString().Replace("\\", ""));
        var WorkHours = JsonConvert.DeserializeObject(t.workHours[day].ToString());
        return WorkHours;
    }
    #region AddDeleteUpdate
    public async Task<ConfigurationParameter> Add(ConfigurationParameter item)
    {
        ConfigurationParameter configuration = await _context.ConfigurationParameter
            .Where(i => (i.Key == item.Key)).SingleOrDefaultAsync();

        if (configuration != null)
        {
            return null;
        }

        var entity = _context.ConfigurationParameter.Add(item);
        ConfigurationParameter newConfiguration = entity.Entity;

        bool success = await CompleteAsync() > 0;

        if (success)
        {
            IEnumerable<ConfigurationParameter> configurations = await _context.ConfigurationParameter.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.ConfigurationParameter);
            await _cacheService.SetAsync<IEnumerable<ConfigurationParameter>>(CacheKeys.ConfigurationParameter, configurations).ConfigureAwait(false);
            return newConfiguration;
        }
        else
        {
            return null;
        }
    }

    public async Task<bool> DeleteConfigurationAsync(string id)
    {
        bool success = false;
        ConfigurationParameter item = await _context.ConfigurationParameter.SingleOrDefaultAsync(p => p.Key == id).ConfigureAwait(false);
        _ = _context.ConfigurationParameter.Remove(item);
        success = await _context.SaveChangesAsync() > 0;
        if (success)
        {
            IEnumerable<ConfigurationParameter> configurations = await _context.ConfigurationParameter.ToArrayAsync().ConfigureAwait(false);

            await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.ConfigurationParameter);
            await _cacheService.SetAsync<IEnumerable<ConfigurationParameter>>(CacheKeys.ConfigurationParameter, configurations).ConfigureAwait(false);
        }
        return success;
    }

    public async Task<bool> Update(ConfigurationParameter item)
    {
        _ = _context.ConfigurationParameter.Update(item);
        _context.Entry(item).State = EntityState.Modified;
        try
        {
            bool success = await CompleteAsync() > 0;
            if (success)
            {
                IEnumerable<ConfigurationParameter> configurations = await _context.ConfigurationParameter.ToArrayAsync().ConfigureAwait(false);
                await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.ConfigurationParameter);
                await _cacheService.SetAsync<IEnumerable<ConfigurationParameter>>(CacheKeys.ConfigurationParameter, configurations).ConfigureAwait(false);

            }
            return success;
        }
        catch (Exception ex) 
        {
            return false;
        }
       
    }
    #endregion AddDeleteUpdate
}
