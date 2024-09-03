using System.Xml.Serialization;

namespace Core.Entities.PisData;

[XmlRoot(ElementName = "FramedVehicleJourneyRef")]
public class FramedVehicleJourneyRef
{

    [XmlElement(ElementName = "DataFrameRef")]
    public DateTime DataFrameRef { get; set; }

    [XmlElement(ElementName = "DatedVehicleJourneyRef")]
    public int DatedVehicleJourneyRef { get; set; }
}
