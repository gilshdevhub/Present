using System.Collections.ObjectModel;

namespace Core.Entities.Vouchers;

public class TrainVoucher
{
    public TrainVoucher()
    {
        this.Items = new Collection<TrainStationVoucher>();
    }

    public int Id { get; set; }
    public int TrainId { get; set; }
    public DateTime TrainDate { get; set; }
    public int DepartureStationId { get; set; }
    public TimeSpan DepartureTime { get; set; }
    public int ArrivalStationId { get; set; }
    public TimeSpan ArrivalTime { get; set; }
    public int NOV { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdated { get; set; }
    public ICollection<TrainStationVoucher> Items { get; set; }

    public required Station DepartureStation { get; set; }
    public required Station ArrivalStation { get; set; }
}
