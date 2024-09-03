using System.Collections.ObjectModel;

namespace Core.Entities.Vouchers;

public class Language
{
    public Language()
    {
        this.Synonyms = new Collection<Synonym>();
    }

    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Synonym> Synonyms { get; set; }
}
