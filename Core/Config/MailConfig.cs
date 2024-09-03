namespace Core.Config;

public class MailConfig
{
    public const string MailSettings = "MailSettings";
    public string MailTo { get; set; }
    public string MailFrom { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
}
