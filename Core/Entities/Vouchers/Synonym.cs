namespace Core.Entities.Vouchers;

public class Synonym
{
    public int Id { get; set; }
    public int StationId { get; set; }
    public int LanguageId { get; set; }
    public string? SynonymName { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    public Station? Station { get; set; }
    public Language? Language { get; set; }
}
