using System.Xml.Serialization;

namespace Core.Entities.PisData;

[XmlRoot(ElementName = "VehicleActivity")]
public class VehicleActivity
{

    [XmlElement(ElementName = "RecordedAtTime")]
    public DateTime RecordedAtTime { get; set; }

    [XmlElement(ElementName = "ValidUntilTime")]
    public DateTime ValidUntilTime { get; set; }

    [XmlElement(ElementName = "VehicleMonitoringRef")]
    public string VehicleMonitoringRef { get; set; }

    [XmlElement(ElementName = "ProgressBetweenStops")]
    public ProgressBetweenStops ProgressBetweenStops { get; set; }

    [XmlElement(ElementName = "MonitoredVehicleJourney")]
    public MonitoredVehicleJourney MonitoredVehicleJourney { get; set; }

    [XmlElement(ElementName = "Extensions")]
    public Extensions Extensions { get; set; }
}
