using Core.Entities.PriceEngine;
using Core.Entities.Translation;
using Core.Entities.Vouchers;

namespace Core.Interfaces;

public interface IPriceEngineService
{
    
    Task<GetAllPriceResultObject> GetAllPriceResultAsync();
    Task<GetAllPriceResultObjectWithNotes> GetAllPriceResultWithNotesAsync();
    Task<IEnumerable<priceDetails>> GetPriceResultAsync(getPriceRequest dto);
    Task<IEnumerable<profileDetails>> getProfilesResultAsync(getProfiles dto);
    Task<IEnumerable<PriceNotesResponseDto>> GetAllPriceNotesAsync();
    Task<IEnumerable<Profiles>> GetAllProfiles();
    Task<PriceNotes> AddPriceNoteAsync(PriceNotes item);
    Task<bool> DeletePriceNoteAsync(int id);
    Task<bool> UpdatePriceNoteAsync(PriceNotes item);
}
