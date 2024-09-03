using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Vouchers;

public class ParkingCosts
{
    [ForeignKey("ParkingCostsId")]
    public int Id { get; set; }
    public string Value { get; set; }
    public string Key { get; set; }
}
