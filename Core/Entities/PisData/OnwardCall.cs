using System.Xml.Serialization;

namespace Core.Entities.PisData;

[XmlRoot(ElementName = "OnwardCall")]
public class OnwardCall
{

    [XmlElement(ElementName = "StopPointRef")]
    public int StopPointRef { get; set; }

    [XmlElement(ElementName = "Order")]
    public int Order { get; set; }

    [XmlElement(ElementName = "ExpectedArrivalTime")]
    public DateTime ExpectedArrivalTime { get; set; }

    [XmlElement(ElementName = "ArrivalStatus")]
    public string ArrivalStatus { get; set; }

    [XmlElement(ElementName = "ArrivalPlatformName")]
    public int ArrivalPlatformName { get; set; }
}
