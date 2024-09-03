using Core.Entities;

namespace Core.Interfaces;

public interface IPlanningAndRatesService
{
    Task<IEnumerable<PlanningAndRatesDto>> GetPlanningAndRatesAsync(int categoryId);
    Task<IEnumerable<PlanningAndRatesDto>> GetPlanningAndRatesAsync();
    Task<PlanningAndRatesDto> GetPlanningAndRatesByIdAsync(Guid Id);
    Task<PlanningAndRates> AddPlanningAndRatesAsync(PlanningAndRates exemptionNotices);
    Task<PlanningAndRates> UpdatePlanningAndRatesAsync(PlanningAndRates exemptionNoticesToUpdate);
    Task<bool> DeletePlanningAndRatesAsync(Guid Id);
    Task<IEnumerable<PlanningAndRatesDto>> UpdateCachePlanningAndRatesAsync();
    Task<IEnumerable<PlanningAndRatesDto>> GetNoCache();
}
