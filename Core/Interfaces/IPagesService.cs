using Core.Entities.PagesManagement;

namespace Core.Interfaces;

public interface IPagesService
{
    Task<IEnumerable<Page>> GetPagesAsync();
    Task<IEnumerable<PageResponse>> GetPagesPerUserAsync(string UserName);
    Task<bool> AddPageAsync(Page pageToAdd);
    Task<bool> UpdatePageAsync(Page pageToUpdate);
    Task<bool> DeletePageAsync(int Id);
}