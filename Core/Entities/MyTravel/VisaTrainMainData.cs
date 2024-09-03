namespace Core.Entities.MyTravel;

/// <summary>
///  for Redis Query
/// </summary>


public class VisaTrainMainData
{ 

     
    public int NMBRAK { get; set; }

    public string DTRKNS { get; set; }
    public int SUGRAK { get; set; }

    public int TOTKR { get; set; }
    public int THMOZ { get; set; }

    public int THYAD { get; set; }

    public int SEATPLACES { get; set; }

   
    public string DTHGAA { get; set; }

    public string DTHGAP { get; set; }

    public string DTRKNP { get; set; }

    public string HORHG { get; set; }

    public string HORHGP { get; set; }

    public long ARR_DIFF { get; set; }

    public string ZMYZ { get; set; }

    public string ZMYZP { get; set; }
    public long DEP_DIFF { get; set; }
    public bool ArrivalTomorrow { get; set; }


    public ICollection<StationDataRedisDTO> VisaTrainStationsTime { get; set; }

   
}

public class StationDataRedisDTO
{


    private const int DEFAULT = 0;

    public int NMBRAK { get; set; }


    public string DTRKNS { get; set; }
    public int SHURA2 { get; set; }
    public int THANA { get; set; }
    public string? THTEUR { get; set; }

    public string? DTHYZA { get; set; }

    public string? DTHYZP { get; set; }

    public string? HORHG { get; set; }

    public string? HORHGP { get; set; }
    public long ARR_DIFF { get; set; }

    public string? ZMYZ { get; set; }

    public string? ZMYZP { get; set; }

    public long DEP_DIFF { get; set; }

    public string? COMMERCIAL_STOP { get; set; }
    public int MIKUM { get; set; }

}
