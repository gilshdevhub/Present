using Core.Entities;

namespace Core.Interfaces;

public interface ITendersService
{
    Task<IEnumerable<TendersDto>> GetTendersAsync();
    Task<IEnumerable<TendersDto>> GetTendersAsync(int categoryId);
    Task<IEnumerable<TendersDto>> GetTendersByTypeAsync(int type);
    Task<TendersDto> GetTenderByIdAsync(Guid Id);
    Task<Tenders> AddTendersAsync(Tenders tender);
    Task<Tenders> UpdateTendersAsync(Tenders tendersToUpdate);
    Task<bool> DeleteTendersAsync(Guid Id);
    Task<IEnumerable<TendersDto>> UpdateCacheTendersAsync();
    Task<bool> TenderNumberExist(int Id);

}
