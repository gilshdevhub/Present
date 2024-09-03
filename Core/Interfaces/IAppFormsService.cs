using Core.Entities.Forms;

namespace Core.Interfaces;

public interface IAppFormsService
{
    Task<string> PostFormsAspxAsync(FullForms fullForms);
    Task< FormsResponse> PostFormsAsync(FullForms fullForms);
    Task<IEnumerable<FormsIdThrees>> GetThreesAsync();
    Task<FormsIdThrees> GetThreeByIdAsync(int formId);
    Task<FormsIdThrees> AddFormsIdThreeAsync(FormsIdThrees formsIdThreeToAdd);
    Task<bool> UpdateFormsIdThreesAsync(FormsIdThrees formsIdThreeToUpdate);
    Task<bool> DeleteFormsIdThreeAsync(int formId);
}
