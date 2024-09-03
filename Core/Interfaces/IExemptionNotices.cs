using Core.Entities;

namespace Core.Interfaces;

public interface IExemptionNotices
{
    Task<IEnumerable<ExemptionNoticesDto>> GetExemptionNoticesAsync(int categoryId);
    Task<IEnumerable<ExemptionNoticesDto>> GetExemptionNoticesNoCache();
    Task<IEnumerable<ExemptionNoticesDto>> GetExemptionNoticesAsync();
    Task<ExemptionNoticesDto> GetExemptionNoticesByIdAsync(Guid Id);
    Task<ExemptionNotices> AddExemptionNoticesAsync(ExemptionNotices exemptionNotices);
    Task<ExemptionNotices> UpdateExemptionNoticesAsync(ExemptionNotices exemptionNoticesToUpdate);
    Task<bool> DeleteExemptionNoticesAsync(Guid Id);
    Task<IEnumerable<ExemptionNoticesDto>> UpdateCacheExemptionNoticesAsync();
}
