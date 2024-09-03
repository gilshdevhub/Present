namespace Core.Config;

public class PushwooshConfig
{
    public const string Pushwoosh = "Push.Pushwoosh";

    public string Application { get; set; }
    public string Token { get; set; }
    public string ApiUrl { get; set; }
    public string ContentType { get; set; }
}
