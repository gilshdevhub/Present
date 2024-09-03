using Core.Entities.ContentPages;

namespace Core.Interfaces;

public interface IContentPagesService
{
    Task<ContentPages> GetContentPageAsync(int id);
    Task<IEnumerable<ContentPages>> GetContentPagesAsync();
    Task<ContentPages> AddContentPageAsync(ContentPages contentPageToAdd);
    Task<ContentPages> UpdateContentPageAsync(ContentPages contentPageToUpdate);
    Task<bool> DeleteContentPageAsync(int ContentPageId);

}
