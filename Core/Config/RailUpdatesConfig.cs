namespace Core.Config;

public class RailUpdatesConfig
{
    public const string RailUpdates = "RailUpdates";

    public RailUpdatesBase General { get; set; }
    public RailUpdatesBase GeneralUmbraco { get; set; }
    public RailUpdatesBase Special { get; set; }
    public RailUpdatesBase GeneralStation { get; set; }
    public RailUpdatesBase SpecialStation { get; set; }
    public RailUpdatesUmbraco Umbraco { get; set; }

}

public class RailUpdatesBase
{
    public string ApiUrl { get; set; }
    public string ContentType { get; set; }
}
public class RailUpdatesUmbraco
{
    public string Id { get; set; }
    public string UmbracoUrl { get; set; }
    public string LinkUrl { get; set; }

}
