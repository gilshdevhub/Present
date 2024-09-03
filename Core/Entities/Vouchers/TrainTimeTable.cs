using System.Collections.ObjectModel;

namespace Core.Entities.Vouchers;

public class TrainTimeTable
{
    public TrainTimeTable()
    {
        this.Items = new Collection<TrainStationsTimeTable>();
    }

    public int Id { get; set; }
    public int TrainId { get; set; }
    public string? TrainAlias { get; set; }
    public TimeSpan DepartureTime { get; set; }
    public int ArrivalStationId { get; set; }
    public int DepartureStationId { get; set; }
    public TimeSpan ArrivalTime { get; set; }
    public bool SundayJ { get; set; }
    public bool MondayJ { get; set; }
    public bool TuesdayJ { get; set; }
    public bool WednesdayJ { get; set; }
    public bool ThursdayJ { get; set; }
    public bool FridayJ { get; set; }
    public bool SaturdayJ { get; set; }
    public int SundayNOV { get; set; }
    public int MondayNOV { get; set; }
    public int TuesdayNOV { get; set; }
    public int WednesdayNOV { get; set; }
    public int ThursdayNOV { get; set; }
    public int FridayNOV { get; set; }
    public int SaturdayNOV { get; set; }
    public DateTime TimeTableStartDate { get; set; }
    public DateTime TimeTableEndDate { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdated { get; set; }
    public ICollection<TrainStationsTimeTable> Items { get; set; }

    public required Station ArrivalStation { get; set; }
    public required Station DepartureStation { get; set; }
}
