using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml;

namespace Core.Entities.TimeTable;

public class Train
{
    public Train()
    {
        this.StopStations = new Collection<StopStation>();
        this.RouteStations = new Collection<RouteStation>();
    }
    public Train(XmlNode train, XmlNode tsd, XmlNode tps) : this()
    {
        CultureInfo hebrewCultureInfo = new("he-IL");

        this.TrainNumber = int.Parse(train.SelectSingleNode("Trainno").InnerText);
        this.OrignStation = int.Parse(train.SelectSingleNode("OrignStation").InnerText);
        this.DestinationStation = int.Parse(train.SelectSingleNode("DestinationStation").InnerText);
        this.OriginPlatform = int.Parse(train.SelectSingleNode("Platform").InnerText);
        this.DestPlatform = int.Parse(train.SelectSingleNode("DestPlatform").InnerText);
        this.ArrivalTime = DateTime.Parse(train.SelectSingleNode("ArrivalTime").InnerText, hebrewCultureInfo);
        this.DepartureTime = DateTime.Parse(train.SelectSingleNode("DepartureTime").InnerText, hebrewCultureInfo);
        this.Handicap = int.Parse(train.SelectSingleNode("Handicap").InnerText);
        this.StopStations = GetStopStations(train, tsd);
        this.Crowded = GetStationCroweded(tsd);
        this.TrainPosition = GetTrainPosition(tps);
        this.RouteStations = GetRouteStations(tsd);
    }

    public int TrainNumber { get; set; }
    public int OrignStation { get; set; }
    public int DestinationStation { get; set; }
    public int OriginPlatform { get; set; }
    public int DestPlatform { get; set; }
    public int FreeSeats { get; set; }
    public DateTime ArrivalTime { get; set; }
    public DateTime DepartureTime { get; set; }
    public ICollection<StopStation> StopStations { get; set; }
    public int Handicap { get; set; }
    public decimal Crowded { get; set; }
    public TrainPosition TrainPosition { get; set; }
    public ICollection<RouteStation> RouteStations { get; set; }

    private ICollection<RouteStation> GetRouteStations(XmlNode tsd)
    {
        List<RouteStation> routeStations = new();

        XmlNodeList tsdTrainStations = tsd.SelectNodes($"/TSD/Train[@num={this.TrainNumber}]/Station");

        foreach (XmlNode tsdTrainStation in tsdTrainStations)
        {
            routeStations.Add(new RouteStation(tsdTrainStation));
        }

        return routeStations;
    }
    private TrainPosition GetTrainPosition(XmlNode tps)
    {
        TrainPosition trainPosition = null;

        try
        {
            XmlNode tp = tps.SelectSingleNode($"/TPS/TP[Trainno={this.TrainNumber}]");
            if (tp != null)
            {
                trainPosition = new TrainPosition
                {
                    CalcDiffMinutes = int.Parse(tp.SelectSingleNode("DifMin").InnerText) * (tp.SelectSingleNode("DifType").InnerText == "DELAY" ? 1 : 0),
                    CurrentLastStation = int.Parse(tp.SelectSingleNode("CurLastStation").InnerText),
                    NextStation = int.Parse(tp.SelectSingleNode("NextStation").InnerText)
                };
            }
        }
        catch { }

        return trainPosition;
    }
    private decimal GetStationCroweded(XmlNode tsd)
    {
        decimal crowded = default;

        XmlNode stdTrainStation = tsd.SelectSingleNode($"Train[@num ={this.TrainNumber}]/Station[@Num={this.OrignStation}]");
        if (stdTrainStation != null)
        {
            _ = decimal.TryParse(stdTrainStation.Attributes["Omes"].InnerText, out crowded);
        }

        return crowded;
    }
    private ICollection<StopStation> GetStopStations(XmlNode train, XmlNode tsd)
    {
        List<StopStation> stations = new();

        XmlNodeList stopStations = train.SelectNodes("StopStations/Station");
        XmlNode tsdTrain = tsd.SelectSingleNode($"Train[@num={this.TrainNumber}]");

        foreach (XmlNode stopStation in stopStations)
        {
            stations.Add(new StopStation(stopStation, tsdTrain));
        }

        return stations;
    }
}
