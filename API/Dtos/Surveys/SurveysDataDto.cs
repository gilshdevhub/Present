using System;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Surveys;

public class SurveysDataDto
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }
    [MaxLength(200)]
    public string Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}
