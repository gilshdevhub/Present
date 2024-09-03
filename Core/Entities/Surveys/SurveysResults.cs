using Core.Entities.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Surveys;

public class SurveysResults
{
    [Required]
    public DateTime TimeStamp { get; set; }
    [Required]
    [Range(minimum: 10, maximum: 12)]

    public string CustID { get; set; }
    [Required]
    public int SystemTypeId { get; set; }
    [Required]
    [Range(minimum: 1, maximum: 5)]
    public int Score { get; set; }
    [Required]
    public int SurveyId { get; set; }
    public SystemType SystemType { get; set; }

}
