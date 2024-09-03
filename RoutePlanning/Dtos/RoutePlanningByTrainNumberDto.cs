using System.ComponentModel.DataAnnotations;

namespace RoutePlanning.Dtos;

public class RoutePlanningByTrainNumberRequest : RoutePlanningRequest
{
    [Required]
    [Range(minimum: 1, maximum: 9999)]
    public int TrainNumber { get; set; }
}
