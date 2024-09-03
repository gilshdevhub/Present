namespace Core.Entities.ContentPages;

public class ContentPages
{
    public int Id { get; set; }
    public string Content { get; set; }
    public string Header { get; set; }
    public string Footer { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdate { get; set; }
}
