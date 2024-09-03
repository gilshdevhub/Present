using System.Xml.Serialization;

namespace Core.Entities.PisData;

[XmlRoot(ElementName = "OnwardCalls")]
public class OnwardCalls
{

    [XmlElement(ElementName = "OnwardCall")]
    public List<OnwardCall> OnwardCall { get; set; }
}
