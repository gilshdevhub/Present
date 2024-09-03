using Core.Entities.Configuration;
using Core.Enums;

namespace Core.Interfaces;

public interface IConfigurationService
{
    Task<IEnumerable<ConfigurationParameter>> GetAllItemsAsync();
    Task<IEnumerable<ConfigurationParameter>> GetAllItemsNoCache();
    Task<Configuration> GetItemAsync(string key, SystemTypes systemType);
    Task<ConfigurationParameter> GetItemAsync(string key);
    Task<ConfigurationParameter> Add(ConfigurationParameter item);
    Task<bool> Update(ConfigurationParameter item);
    Task<bool> DeleteConfigurationAsync(string id);
    Task<IEnumerable<Configuration>> GetConfigurationsBySystemTypeAsync(SystemTypes SystemTypeId);
    Task<IEnumerable<SystemType>> GetSystemTypesAsync();
    Task<object> GetWeekDaysAsync(DateTime date);
}
