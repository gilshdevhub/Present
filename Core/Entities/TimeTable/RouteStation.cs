using System.Xml;

namespace Core.Entities.TimeTable;

public class RouteStation
{
    public RouteStation(XmlNode tsdTrainStation)
    {
        if (decimal.TryParse(tsdTrainStation.Attributes["Omes"].InnerText, out decimal crowded))
        {
            this.Crowded = crowded;
        }
        this.StationId = int.Parse(tsdTrainStation.Attributes["Num"].InnerText);
        this.ArrivalTime = tsdTrainStation.Attributes["Time"].InnerText;
        this.Platform = int.Parse(tsdTrainStation.Attributes["Platform"].InnerText);
    }

    public int StationId { get; set; }
    public string ArrivalTime { get; set; }
    public decimal Crowded { get; set; }
    public int Platform { get; set; }
}
