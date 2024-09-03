namespace Core.Entities.VersionCatalog;

public class VersionCatalog
{
    public int Id { get; set; }
    public string Description { get; set; }
    public int VersionId { get; set; }
    public DateTime? LastUpdated { get; set; }
}
