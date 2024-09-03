using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Push;

public class PushNotificationAndRegistrationIds
{
    [Key]
    public int PushNotId { get; set; }
    public int Permanent { get; set; }
       public int TrainNumber { get; set; }
    public DateTime DepartureTime { get; set; }    public string DepartureStationName { get; set; }
    public DateTime ArrivalTime { get; set; }      public string ArrivalStationName { get; set; }
    public int DepartureStationId { get; set; }    public int ArrivalStationId { get; set; }      public int DepartutePlatform { get; set; }
    public int ArrivalPlatform { get; set; }
       public int PushRegistrationId { get; set; }

}

public class FilterParametrs
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string TrainNumbers { get; set; } = "";
    public int DepartureStationId { get; set; } = 0;
    public int ArrivalStationId { get; set; } = 0;
}

public class FilterParametrsMaslul
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int TrainNumber { get; set; } = 0;
    public int StartStationId { get; set; } = 0;
    public int EndStationId { get; set; } = 0;
}
