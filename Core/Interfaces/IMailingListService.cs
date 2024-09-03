using Core.Entities;

namespace Core.Interfaces;

public interface IMailingListService
{
    Task<IEnumerable<MailingList>> GetMailingListAsync();
    Task<MailingList> AddMailingListAsync(MailingList mailingList);
    Task<MailingList> UpdateMailingListAsync(MailingList mailingListToUpdate);
    Task<bool> DeleteMailingListAsync(int Id);
    Task<MailingList?> GetMailingListByIdAsync(int Id);
    Task<bool> DeleteSingleMailAsync(string mail);
    Task<IEnumerable<MailingList>> GetMailingListsContentNoCache();
    Task<MailingList> GetMailingListByIdNoCache(int Id);

}
