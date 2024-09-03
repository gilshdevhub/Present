using Core.Entities.Identity;
using Core.Entities.PagesManagement;

namespace Core.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<AppRole>> GetRolesAsync();
    Task<bool> AddOrUpdatePageRolesAsync(ICollection<PageRoleNewResponse> pagesroles, int roleId);
}