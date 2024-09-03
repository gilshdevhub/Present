namespace Core.Entities.UniCell;

public class UniCellSendData
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string SenderName { get; set; }
    public string BodyMessage { get; set; }
    public int Relative { get; set; }
    public int RootReference { get; set; }
    public bool Ist2s { get; set; }
    public List<UniCellRecipient> Recipients { get; set; }

}
public class UniCellRecipient
{
    public string Cellphone { get; set; }
    public string Reference { get; set; }
    public string TemplateID { get; set; }
}
public class UniCellResponse
{
    public int StatusCode { get; set; }
    public string Description { get; set; }
    public List<UniCellResponseReference> References { get; set; }
}
public class UniCellResponseReference
{
    public string ReferenceNumber { get; set; }
    public string Cellphone { get; set; }
}
