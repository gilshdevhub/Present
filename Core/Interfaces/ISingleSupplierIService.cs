using Core.Entities;

namespace Core.Interfaces;

public interface ISingleSupplierIService
{
    Task<IEnumerable<SingleSupplierDto>> GetSuppliersAsync();
    Task<IEnumerable<SingleSupplierDto>> GetSuppliersNoCache();
    Task<IEnumerable<SingleSupplierDto>> GetSuppliersAsync(int categoryId);
    Task<IEnumerable<SingleSupplierDto>> GetSuppliersNoCache(int categoryId);
    Task<SingleSupplier> AddSupplierAsync(SingleSupplier supplier);
    Task<SingleSupplier> UpdateSupplierAsync(SingleSupplier supplierToUpdate);
    Task<bool> DeleteSupplierAsync(Guid Id);
    Task<SingleSupplierDto> GetSupplierByIdAsync(Guid Id);
    Task<IEnumerable<SingleSupplierDto>> UpdateCacheSingleSupplierAsync();
}
