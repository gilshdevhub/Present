using Microsoft.EntityFrameworkCore;

namespace Core.Entities;
[Keyless]
public class TrainLocation
{
    public int TrainNr { get; set; }
    public DateTime TravelDate { get; set;}
    public string UID { get; set;}
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}
