namespace Core.Entities.Messenger;

public class InforuResponse
{
    public Result Result { get; set; }
}

public class Result
{
    public int Status { get; set; }
    public string Description { get; set; }
    public int NumberOfRecipients { get; set; }
}
