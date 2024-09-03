namespace Core.Config;

public class UniCellConfig
{
    public const string UniCell = "SMS:UniCell";
    public string UserName { get; set; }
    public string Password { get; set; }
    public string ApiUrl { get; set; }
    public string SenderName { get; set; }
    public int Relative { get; set; }
    public int RootReference { get; set; }
    public bool Ist2s { get; set; }
}
