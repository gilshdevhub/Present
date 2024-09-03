namespace Core.Entities.FreeSeats;

public class FreeSeatsRequest
{
    public IEnumerable<int> TrainNumbers { get; set; }
    public string TrainDate { get; set; }
    public int DestinationStation { get; set; }
    public int OriginStation { get; set; }
}
