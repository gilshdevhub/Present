using System.Xml.Serialization;

namespace Core.Entities.PisData;

[XmlRoot(ElementName = "VehicleMonitoringDelivery")]
public class VehicleMonitoringDelivery
{

    [XmlElement(ElementName = "ResponseTimestamp")]
    public DateTime ResponseTimestamp { get; set; }

    [XmlElement(ElementName = "Status")]
    public bool Status { get; set; }

    [XmlElement(ElementName = "ValidUntil")]
    public DateTime ValidUntil { get; set; }

    [XmlElement(ElementName = "VehicleActivity")]
    public List<VehicleActivity> VehicleActivity { get; set; }

    [XmlAttribute(AttributeName = "version")]
    public string Version { get; set; }

    [XmlText]
    public string Text { get; set; }
}
