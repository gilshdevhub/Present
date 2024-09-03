using System.Xml.Serialization;

namespace Core.Entities.PisData;

[XmlRoot(ElementName = "PreviousCalls")]
public class PreviousCalls
{

    [XmlElement(ElementName = "PreviousCall")]
    public List<PreviousCall> PreviousCall { get; set; }
}
