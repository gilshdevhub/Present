namespace Core.Entities.Messenger;

public class TelemessageResponse
{
    public Telemassage Telemessage { get; set; }
}

public class Telemassage
{
    public TelemessageContent Telemessage_Content { get; set; }
    public int Version { get; set; }
}

public class TelemessageContent
{
    public Response Response { get; set; }
}

public class Response
{
    public int Message_ID { get; set; }
    public string Message_Key { get; set; }
    public int Response_Status { get; set; }
    public string Response_Status_Desc { get; set; }
}
