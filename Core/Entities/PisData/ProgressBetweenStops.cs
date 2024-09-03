using System.Xml.Serialization;

namespace Core.Entities.PisData;

[XmlRoot(ElementName = "ProgressBetweenStops")]
public class ProgressBetweenStops
{

    [XmlElement(ElementName = "LinkDistance")]
    public int LinkDistance { get; set; }

    [XmlElement(ElementName = "Percentage")]
    public int Percentage { get; set; }
}
