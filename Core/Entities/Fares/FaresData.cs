namespace Core.Entities.Fares;

public class FaresData
{
    public Languages Title { get; set; }
    public Languages ContractTitleCount { get; set; }
    public int VersionId { get; set; }
    public IEnumerable<Result> Results { get; set; }
}
public class Languages
{
    public string He { get; set; }
    public string EN { get; set; }
    public string RU { get; set; }
    public string AR { get; set; }
}

public class Result
{
    public Languages Title { get; set; }
    public Languages SubTitle { get; set; }
    public IEnumerable<ContractsResult> ContractsResults { get; set; }
}

public class ContractsResult
{
    public string Trip_Type { get; set; }
    public int ETT_Code { get; set; }
    public int PREDEFINE { get; set; }
    public int Metro_Code { get; set; }
    public int Station_Code_Origin { get; set; }
    public int Station_Code_Dest { get; set; }
    public int SmartCard_Profile_Code { get; set; }
    public int ContractCodeMagnetic { get; set; }
    public int Magnetic_Profile_Code { get; set; }
    public int Contract_Code { get; set; }
    public string Contract_Name_He { get; set; }
    public string Contract_Name_En { get; set; }
    public string Contract_Name_Ar { get; set; }
    public string Contract_Name_Ru { get; set; }
    public string Price { get; set; }
    public bool IsAvailableForSale { get; set; }
    public string RestrictionTitle_HE { get; set; }
    public string RestrictionTitle_EN { get; set; }
    public string RestrictionTitle_RU { get; set; }
    public string RestrictionTitle_AR { get; set; }
}

public class FaresRequestData
{
    public int Profile_Code { get; set; }
    public int Station_Origin_Code { get; set; }
    public int Station_Destination_Code { get; set; }
    public int ETT_Code { get; set; }
}
