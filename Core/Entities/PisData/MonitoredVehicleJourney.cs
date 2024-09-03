using System.Xml.Serialization;

namespace Core.Entities.PisData;

[XmlRoot(ElementName = "MonitoredVehicleJourney")]
public class MonitoredVehicleJourney
{

    [XmlElement(ElementName = "LineRef")]
    public int LineRef { get; set; }

    [XmlElement(ElementName = "DirectionRef")]
    public int DirectionRef { get; set; }

    [XmlElement(ElementName = "FramedVehicleJourneyRef")]
    public FramedVehicleJourneyRef FramedVehicleJourneyRef { get; set; }

    [XmlElement(ElementName = "PublishedLineName")]
    public string PublishedLineName { get; set; }

    [XmlElement(ElementName = "OperatorRef")]
    public int OperatorRef { get; set; }

    [XmlElement(ElementName = "OriginRef")]
    public int OriginRef { get; set; }

    [XmlElement(ElementName = "DestinationRef")]
    public int DestinationRef { get; set; }

    [XmlElement(ElementName = "OriginAimedDepartureTime")]
    public DateTime OriginAimedDepartureTime { get; set; }

    [XmlElement(ElementName = "Monitored")]
    public bool Monitored { get; set; }

    [XmlElement(ElementName = "ConfidenceLevel")]
    public string ConfidenceLevel { get; set; }

    [XmlElement(ElementName = "VehicleLocation")]
    public VehicleLocation VehicleLocation { get; set; }

    [XmlElement(ElementName = "Bearing")]
    public int Bearing { get; set; }

    [XmlElement(ElementName = "Velocity")]
    public int Velocity { get; set; }

    [XmlElement(ElementName = "VehicleRef")]
    public int VehicleRef { get; set; }

    [XmlElement(ElementName = "PreviousCalls")]
    public PreviousCalls PreviousCalls { get; set; }

    [XmlElement(ElementName = "MonitoredCall")]
    public MonitoredCall MonitoredCall { get; set; }

    [XmlElement(ElementName = "OnwardCalls")]
    public OnwardCalls OnwardCalls { get; set; }
}
