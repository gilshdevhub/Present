namespace Core.Config;

public class FirebaseCredentialConfig
{
    public const string Firebase = "Push:Firebase";

    public string? Type { get; set; }
    public string? project_id { get; set; }
    public string? client_email { get; set; }
    public string? client_id { get; set; }
    public string? private_key { get; set; }
}
