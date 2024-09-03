using System.Xml.Serialization;

namespace Core.Entities.PisData;

[XmlRoot(ElementName = "PreviousCall")]
public class PreviousCall
{

    [XmlElement(ElementName = "StopPointRef")]
    public int StopPointRef { get; set; }

    [XmlElement(ElementName = "Order")]
    public int Order { get; set; }

    [XmlElement(ElementName = "ActualArrivalTime")]
    public DateTime ActualArrivalTime { get; set; }

    [XmlElement(ElementName = "ActualDepartureTime")]
    public DateTime ActualDepartureTime { get; set; }
}
