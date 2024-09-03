using System.Collections.ObjectModel;

namespace Core.Entities.Vouchers;

public class DailyTrainsTimeTable
{
    public DailyTrainsTimeTable()
    {
        this.Items = new Collection<DailyTrainStationsTimeTable>();
    }

    public int TrainId { get; set; }
    public DateTime TrainDate { get; set; }
    public int DepartureStationId { get; set; }
    public TimeSpan DepartureTime { get; set; }
    public int ArrivalStationId { get; set; }
    public TimeSpan ArrivalTime { get; set; }
    public string? ChangeType { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdated { get; set; }
    public ICollection<DailyTrainStationsTimeTable> Items { get; set; }

    public required Station ArrivalStation { get; set; }
    public required Station DepartureStation { get; set; }
}
