using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Fares;

public class EttCodes
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int ETT_Code { get; set; }
    public bool RelevantForMetropoline { get; set; }
    public string ETT_Name_He { get; set; }
    public string ETT_Name_En { get; set; }
    public string ETT_Name_Ar { get; set; }
    public string ETT_Name_Ru { get; set; }
}

public class ProfileCodes
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int SmartCard_Profile_Code { get; set; }
    public string Profile_Name_He { get; set; }
    public string Profile_Name_En { get; set; }
    public string Profile_Name_Ar { get; set; }
    public string Profile_Name_Ru { get; set; }
    public int Profile_Magnetic { get; set; }
}

public class CodesDataDto
{
    public IEnumerable<EttCodes> ettCodes { get; set; }
    public IEnumerable<ProfileCodes> profileCodes { get; set; }
}

public class FaresVersions
{
    public int Id { get; set; }
    public int Version { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}