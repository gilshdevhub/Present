using Core.Entities;

namespace Core.Interfaces;

public interface IManagmentSystemObjects
{
    Task<List<ManagmentSystemObjects>> GetManagmentObjectsAsync();
    Task<ManagmentSystemObjects?> GetManagmentObjectsByIdAsync(int Id);
    Task<ManagmentSystemObjects?> GetManagmentObjectsByNameAsync(string Name);
    Task<ManagmentSystemObjects> UpdateManagmentObjectsByIdAsync(int Id, string value);
    Task<IEnumerable<ManagmentSystemObjects>> GetManagmentSystemObjectsAsync();
}
