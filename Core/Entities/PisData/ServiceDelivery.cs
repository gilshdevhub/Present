using System.Xml.Serialization;

namespace Core.Entities.PisData;

[XmlRoot(ElementName = "ServiceDelivery")]
public class ServiceDelivery
{

    [XmlElement(ElementName = "ResponseTimestamp")]
    public DateTime ResponseTimestamp { get; set; }

    [XmlElement(ElementName = "ProducerRef")]
    public string ProducerRef { get; set; }

    [XmlElement(ElementName = "ResponseMessageIdentifier")]
    public string ResponseMessageIdentifier { get; set; }

    [XmlElement(ElementName = "VehicleMonitoringDelivery")]
    public VehicleMonitoringDelivery VehicleMonitoringDelivery { get; set; }
}
