namespace Core.Entities;

public class Meetings
{
    public int MeetingsId { get; set; }
    public DateTime MeetingDate { get; set; }
    public string RegistretionLink { get; set; }
    public string Location { get; set; }
    public Guid TendersId { get; set; }
    public virtual Tenders Tenders { get; set; }
}
public class MeetingsDto
{
    public int MeetingsId { get; set; }
    public DateTime MeetingDate { get; set; }
    public string RegistretionLink { get; set; }
    public string Location { get; set; }
   
}
