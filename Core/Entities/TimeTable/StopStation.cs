using System.Globalization;
using System.Xml;

namespace Core.Entities.TimeTable;

public class StopStation
{
    public StopStation(XmlNode stopStation, XmlNode tsdTrain)
    {
        CultureInfo hebrewCultureInfo = new("he-IL");

        this.StationId = int.Parse(stopStation.SelectSingleNode("StationId").InnerText);
        this.ArrivalTime = DateTime.Parse(stopStation.SelectSingleNode("ArrivalTime").InnerText, hebrewCultureInfo);
        this.DepartureTime = DateTime.Parse(stopStation.SelectSingleNode("DepartureTime").InnerText, hebrewCultureInfo);
        this.Platform = int.Parse(stopStation.SelectSingleNode("Platform").InnerText);

        XmlNode stdTrainStation = tsdTrain.SelectSingleNode($"Station[@Num={this.StationId}]");
        decimal omes;
        if (decimal.TryParse(stdTrainStation?.Attributes["Omes"].InnerText, out omes))
        {
            this.Crowded = omes;
        }
    }

    public int StationId { get; set; }
    public DateTime ArrivalTime { get; set; }
    public DateTime DepartureTime { get; set; }
    public int Platform { get; set; }
    public decimal Crowded { get; set; }
}
