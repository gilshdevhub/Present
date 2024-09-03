namespace Core.Entities.Notifications;

public class TravelMessage
{
    public int TrainNumber { get; set; }
    public int Sevirity { get; set; }
    public string Message { get; set; }
    public string Title { get; set; }
}
