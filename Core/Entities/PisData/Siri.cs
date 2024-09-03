using System.Xml.Serialization;

namespace Core.Entities.PisData;

public class SiriMain
{
    public Siri Siri { get; set; }
}
[XmlRoot(ElementName = "Siri")]
public class Siri
{

    [XmlElement(ElementName = "ServiceDelivery")]
    public ServiceDelivery ServiceDelivery { get; set; }

}
