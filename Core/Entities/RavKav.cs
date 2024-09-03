using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class RavKav
{
    public int Id { get; set; } = 0;
    [Column(TypeName = "varchar(15)")]
    public string RavKavNumber { get; set; } = "0";
    public float Amount { get; set; } = 0;
    public bool Refundable { get; set; } = false;
}
