namespace SiteManagement.Dtos;

public class TendersTypesPostDto
{
    public byte Type { get; set; }
    public string Name { get; set; }
}
public class TendersTypesPutDto
{
    public int Id { get; set; }
    public byte Type { get; set; }
    public string Name { get; set; }
}
