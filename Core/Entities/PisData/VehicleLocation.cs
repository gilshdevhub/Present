using System.Xml.Serialization;

namespace Core.Entities.PisData;

[XmlRoot(ElementName = "VehicleLocation")]
public class VehicleLocation
{

    [XmlElement(ElementName = "Longitude")]
    public double Longitude { get; set; }

    [XmlElement(ElementName = "Latitude")]
    public double Latitude { get; set; }
}
