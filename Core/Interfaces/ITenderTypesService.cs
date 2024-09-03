using Core.Entities;

namespace Core.Interfaces;

public interface ITenderTypesService
{
    Task<IEnumerable<TenderTypes>> GetTenderTypesAsync();
    Task<IEnumerable<TenderTypes>> GetTenderTypesNoCache();
    Task<TenderTypes?> AddTenderTypesAsync(TenderTypes tenderType);
    Task<TenderTypes?> UpdateTenderTypesAsync(TenderTypes tenderTypeToUpdate);
    Task<bool> DeleteTenderTypesAsync(int Id);
    Task<TenderTypes?> GetTenderTypeByIdAsync(int Id);
    Task<TenderTypes?> GetTenderTypeByIdNoCache(int Id);
}
