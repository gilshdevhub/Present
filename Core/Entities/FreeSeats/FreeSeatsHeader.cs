using System.Collections.ObjectModel;

namespace Core.Entities.FreeSeats;

public class FreeSeatsHeaderRequest
{
    public FreeSeatsHeaderRequest()
    {
        this.FreeSeats = new Collection<FreeSeatsHeader>();
    }

    [Newtonsoft.Json.JsonProperty(propertyName: "lstTrainAvailableChairsQuery")]
    public ICollection<FreeSeatsHeader> FreeSeats { get; set; }
}

public class FreeSeatsHeader
{
    public int TrainNumber { get; set; }
    public string TrainDate { get; set; }
    public int DestStation { get; set; }
    public int FromStation { get; set; }
}
