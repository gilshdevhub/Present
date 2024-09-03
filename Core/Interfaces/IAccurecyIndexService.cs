using Core.Entities.AccurecyIndex;

namespace Core.Interfaces;

public interface IAccurecyIndexService
{
    Task<AccurecyIndexFilteredData> GetAccurecyIndexDataAsync();
}
