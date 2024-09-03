using Core.Entities.AppMessages;

namespace Core.Interfaces;

public interface IPopUpMessagesService
{
    Task<IEnumerable<PopUpMessages>> GetMessagesAsync();
    Task<IEnumerable<PopUpMessages>> GetMessagesAsync(MessageRequest messageRequest);
    Task<bool> AddMessageAsync(PopUpMessages message);
    Task<IEnumerable<PageType>> GetPageTypesAsync();
    Task<bool> UpdateMessageAsync(PopUpMessages messageToUpdate);
    Task<bool> DeleteMessageAsync(int Id);
    Task<bool> DeleteMessageByStationAsync(int StationId);
    Task<IEnumerable<PopUpMessages>> GetMessagesByStationAsync(int StationId);
}
