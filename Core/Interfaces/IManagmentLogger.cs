using Core.Entities.ManagmentLogger;


namespace Core.Interfaces;

public interface IManagmentLogger
{
    Task<ManagmentLog> AddLogAsync(ManagmentLog managmentLog);
    Task<bool> DeleteLogAsync(int Id);
    Task<IEnumerable<ManagmentLog>> GetAllLogAsync();
}
