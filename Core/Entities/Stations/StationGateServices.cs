namespace Core.Entities.Vouchers;

public class StationGateServices
{
    public StationGateServices(int StationGateId, int ServiceId, bool isServiceExist)
    {
        this.StationGateId = StationGateId;
        this.ServiceId = ServiceId;
        this.isServiceExist = isServiceExist;

           }
    public int StationGateId { get; set; }
    public int ServiceId { get; set; }
    public bool isServiceExist { get; set; }
    public StationServices StationServices { get; set; }


}