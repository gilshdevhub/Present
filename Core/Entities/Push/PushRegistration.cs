using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Core.Entities.Push;

public class PushRegistration
{
    public PushRegistration()
    {
        this.PushRoutings = new Collection<PushRouting>();
    }

    public int Id { get; set; }
    public string TokenId { get; set; }
    public string HWId { get; set; }
    public DateTime RegistrationDate { get; set; }
    public int State { get; set; }
    public Nullable<DateTime> CancelDate { get; set; }
    public ICollection<PushRouting> PushRoutings { get; set; }
    [Column(TypeName = "datetime2")]
    [DefaultValue("0001-01-01T00:00:00.0000000")]
    public DateTime RefreshDate { get; set; }
}



public class TokenRefreshModel
{
    public int Id { get; set; }
    public string OldToken { get; set; }
    public string NewToken { get; set; }
}