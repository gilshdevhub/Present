namespace Core.Config;

public class TelemassageConfig
{
    public const string Telemessage = "SMS:Telemassage";

    public string UserName { get; set; }
    public string Password { get; set; }
    public string ApiUrl { get; set; }
    public string ContentType { get; set; }
}
