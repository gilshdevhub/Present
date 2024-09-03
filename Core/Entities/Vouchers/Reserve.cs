namespace Core.Entities.Vouchers;

public class ReserveUrlRequest
{
    public int NumberOfSeats { get; set; }
    public bool SendEmail { get; set; }
    public string Token { get; set; }
}

public class ReserveRequest
{
    public string SmartCard { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public IEnumerable<TrainDetail> TrainsResult { get; set; }
}

public class TrainDetail
{
    public string TrainDate { get; set; }
    public int DestinationStationId { get; set; }
    public string DestinationStationHe { get; set; }
    public int OrignStationId { get; set; }
    public string OrignStationHe { get; set; }
    public int TrainNumber { get; set; }
    public string DepartureTime { get; set; }
    public string ArrivalTime { get; set; }
    public string OrignStation { get; set; }
    public string DestinationStation { get; set; }
    public int OrignStationNum { get; set; }
    public int DestinationStationNum { get; set; }
    public int DestPlatform { get; set; }
    public int TrainOrder { get; set; }
}

public class ReserveResponse
{
    public string QRString { get; set; }
    public bool IsSucceed { get; set; }
    public string VoucherUniqueToken { get; set; }
    public string ErrorCode { get; set; }
    public string ErrorDescription { get; set; }
}
