using System.Xml.Serialization;

namespace Core.Entities.PisData;

[XmlRoot(ElementName = "Extensions")]
public class Extensions
{

    [XmlElement(ElementName = "EndOfTripReason")]
    public string EndOfTripReason { get; set; }
}
