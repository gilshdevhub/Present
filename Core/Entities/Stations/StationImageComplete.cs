using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Stations;

public class StationImageComplete
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Code { get; set; }
    public string? CodeHe { get; set; }
    public string? CodeEn { get; set; }
    public string? CodeRu { get; set; }
    public string? CodeAR { get; set; }
}
//public class StationImageCompleteByLang
//{
//    [Key]
//    public int Id { get; set; }
//    [Required]
//    public string Code { get; set; }
//    [Required]
//    public string CodeHe { get; set; }
//    [Required]    
//    public string CodeEn { get; set; }
//    [Required]    
//    public string CodeRu { get; set; }
//    [Required]    
//    public string CodeAR { get; set; }
//}