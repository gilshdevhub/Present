namespace Core.Entities;

public class TendersUpdates
{
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public string Type { get; set;}
    public DateTime PublishDate { get; set; }
    public int Domain { get; set; }
    public int TypeOfTender { get; set; }
    public int Page { get; set; }
    public int Step { get; set; }
}
