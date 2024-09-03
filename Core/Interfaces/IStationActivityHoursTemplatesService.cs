using Core.Entities.Vouchers;

namespace Core.Interfaces;

public interface IStationActivityHoursTemplatesService
{
    Task<IEnumerable<StationActivityHoursTemplateLinesDto>> GetStationActivityHoursTemplateLinesAsync();
    Task<IEnumerable<StationActivityTemplatesTypes>> GetStationActivityTemplatesTypesNoCache();
    Task<IEnumerable<StationActivityTemplatesTypes>> GetStationActivityTemplatesTypesAsync();
    Task<IEnumerable<TemplatesDto>> GetTamplatesByType(int tamplateId);
    Task<StationActivityHoursTemplates> AddTemplateAsync(StationActivityHoursTemplates stationActivityHoursTemplates, IEnumerable<StationActivityHoursTemplatesLine> stationActivityHoursTemplatesLines);
    Task<bool> UpdateTemplateAsync(StationActivityHoursTemplates stationActivityHoursTemplates, IEnumerable<StationActivityHoursTemplatesLine> stationActivityHoursTemplatesLines, List<int> DeletedIds);
    Task<bool> DeleteTemplateAsync(int Id);
}
