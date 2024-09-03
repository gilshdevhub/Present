using Core.Entities.Identity;
using Core.Entities.PagesManagement;
using Core.Interfaces;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Services;

public class RoleService : IRoleService
{
    private readonly IdentityDbContext _identityDbContext;

    public RoleService(IdentityDbContext identityDbContext)
    {
        _identityDbContext = identityDbContext;
    }


    public async Task<IEnumerable<AppRole>> GetRolesAsync()
    {
        var roles = await _identityDbContext
            .Roles
            .Include(p => p.PageRoleNew)
            .ThenInclude(x => x.Page)
                       .ToArrayAsync();
        return roles;
    }

    public async Task<bool> AddOrUpdatePageRolesAsync(ICollection<PageRoleNewResponse> pagesroles, int roleId)
    {
        bool success;
        foreach (var item in pagesroles)
        {
            var temp = _identityDbContext.PageRoleNew.SingleOrDefault(x => x.PageId == item.PageId && x.RoleId == roleId);
            if (temp != null)
            {

                _ = _identityDbContext.PageRoleNew.Remove(temp);
                success = _identityDbContext.SaveChanges() > 0;
                if (!success)
                {
                    return success;
                }
            }
            PageRoleNew obj = new(roleId, item.PageId, item.Readable, item.Updatable, item.Visible);
            EntityEntry<PageRoleNew> res = await _identityDbContext.PageRoleNew.AddAsync(obj).ConfigureAwait(false);
            success = await _identityDbContext.SaveChangesAsync() > 0;

            if (!success)
            {
                return success;
            }
        }

        return true;
    }
}
