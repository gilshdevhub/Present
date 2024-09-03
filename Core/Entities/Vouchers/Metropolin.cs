using System.Collections.ObjectModel;

namespace Core.Entities.Vouchers;

public class Metropolin
{
    public Metropolin()
    {
        this.Stations = new Collection<Station>();
    }

    public int Id { get; set; }
    public string? Name { get; set; }

    public ICollection<Station> Stations { get; set; }
}
