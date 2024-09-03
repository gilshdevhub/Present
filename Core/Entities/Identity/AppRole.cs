using Core.Entities.PagesManagement;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity;

public class AppRole : IdentityRole<int>
{
    public ICollection<PageRoleNew> PageRoleNew { get; set; }
}
