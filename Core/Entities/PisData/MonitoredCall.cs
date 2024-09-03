using System.Xml.Serialization;

namespace Core.Entities.PisData;

[XmlRoot(ElementName = "MonitoredCall")]
public class MonitoredCall
{

    [XmlElement(ElementName = "StopPointRef")]
    public int StopPointRef { get; set; }

    [XmlElement(ElementName = "Order")]
    public int Order { get; set; }

    [XmlElement(ElementName = "VehicleAtStop")]
    public bool VehicleAtStop { get; set; }

    [XmlElement(ElementName = "ActualArrivalTime")]
    public DateTime ActualArrivalTime { get; set; }

    [XmlElement(ElementName = "ActualDepartureTime")]
    public DateTime ActualDepartureTime { get; set; }

    [XmlElement(ElementName = "AimedDepartureTime")]
    public DateTime AimedDepartureTime { get; set; }
}
