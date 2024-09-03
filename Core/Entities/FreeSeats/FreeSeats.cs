namespace Core.Entities.FreeSeats;

public class FreeSeats
{
    [Newtonsoft.Json.JsonProperty(propertyName: "ListTrainAvailableChairs")]
    public IEnumerable<TrainFreeSeat> TrainAvailableSeats { get; set; }

    public ResultNode clsResult { get; set; }

    public class TrainFreeSeat
    {
        public DateTime TrainDate { get; set; }
        public int TrainNumber { get; set; }
        public int SeatsAvailable { get; set; }
    }

    public class ResultNode
    {
        public int ReturnCode { get; set; }
        public string ReturnDescription { get; set; }
    }
}
