using System.ComponentModel.DataAnnotations;


namespace SiteManagement.Dtos;

public class SurveysResultsDto
{
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    [MaxLength(12), MinLength(10)]
    public string CustID { get; set; }
    [Required]
    public int SystemTypeId { get; set; }
    [Required]
    [Range(minimum: 1, maximum: 5)]
    public int Score { get; set; }
    [Required]
    public int SurveyId { get; set; }
}
 
