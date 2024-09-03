using Core.Entities.Identity;

namespace Core.Entities.PagesManagement;

public class Page
{
    public int Id { get; set; }
    public string? Folder { get; set; }
    public string? Title { get; set; }
    public string? FrontPath { get; set; }
    public string? Component { get; set; }
    public string? ClassProp { get; set; }
    public string? EditComponent { get; set; }
    public string? Controller { get; set; }
    public ICollection<PageRoleNew>? PageRoleNew { get; set; }
}
public class PageResponse
{
    public int Id { get; set; }
    public string? Folder { get; set; }
    public string? Title { get; set; }
    public string? FrontPath { get; set; }
    public string? Component { get; set; }
    public string? ClassProp { get; set; }
    public string? EditComponent { get; set; }
    public string? Controller { get; set; }
       public bool Updatable { get; set; }
    public bool Visible { get; set; }

}
public class PageRoleNew
{
    public PageRoleNew(int roleId, int pageId, bool readable, bool updatable, bool visible)
    {
        RoleId = roleId;
        PageId = pageId;
        Readable = readable;
        Updatable = updatable;
        Visible = visible;
    }
    public int RoleId { get; set; }
    public int PageId { get; set; }
    public virtual Page Page { get; set; }
    public virtual AppRole Role { get; set; }
    public bool Readable { get; set; }
    public bool Updatable { get; set; }
    public bool Visible { get; set; }
}

public class PageRoleNewResponse
{
    public int PageId { get; set; }
    public bool Readable { get; set; }
    public bool Updatable { get; set; }
    public bool Visible { get; set; }
}

