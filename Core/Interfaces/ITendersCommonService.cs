using Core.Entities;

namespace Core.Interfaces;

public interface ITendersCommonService
{
    Task<IEnumerable<TenderDocuments>> GetTenderDocumentsContentAsync();

}
