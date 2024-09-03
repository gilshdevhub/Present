using Core.Entities.Notifications;
using System.Collections.ObjectModel;
using System.Xml;

namespace Core.Entities.TimeTable;

public class Travel
{
    public Travel()
    {
        this.Trains = new Collection<Train>();
        this.TravelMessages = new Collection<TravelMessage>();
    }

    public Travel(XmlNode travel, XmlNode tsd, XmlNode tps) : this()
    {
        XmlNodeList trains = travel.SelectNodes("train");
        foreach (XmlNode train in trains)
        {
            this.Trains.Add(new Train(train, tsd, tps));
        }

        this.DepartureTime = this.Trains.First().DepartureTime;
        this.ArrivalTime = this.Trains.Last().ArrivalTime;
    }

    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public int FreeSeats { get; set; }
    public ICollection<TravelMessage> TravelMessages { get; set; }
    public ICollection<Train> Trains { get; set; }
}
