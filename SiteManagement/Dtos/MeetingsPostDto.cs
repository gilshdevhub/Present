namespace SiteManagement.Dtos;

public class MeetingsPostDto
{
   
    public DateTime MeetingDate { get; set; }
    public string RegistretionLink { get; set; }
    public string Location { get; set; }
    public Guid TendersId { get; set; }
}
public class MeetingsPutDto
{
    public int MeetingsId { get; set; }
    public DateTime MeetingDate { get; set; }
    public string RegistretionLink { get; set; }
    public string Location { get; set; }
    public Guid TendersId { get; set; }
}
