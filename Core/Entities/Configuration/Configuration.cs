namespace Core.Entities.Configuration;

public class Configuration
{
    public int Id { get; set; }
    public int SystemTypeId { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public string Description { get; set; }
    public int LocationId { get; set; }
    public SystemType SystemType { get; set; }
}
