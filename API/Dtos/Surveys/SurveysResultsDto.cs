﻿using System;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Surveys;

public class SurveysResultsDto
{
    [Required]
    public DateTime TimeStamp { get; set; }
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
 
