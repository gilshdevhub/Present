using System.ComponentModel.DataAnnotations;

namespace SiteManagement.Dtos;

public class PushRouteRequestDto
{
    public Nullable<int> PushRegisterId { get; set; }
    [DateTimeValidator(FieldName = "FromDate")]
    public DateTime FromDate { get; set; }
    [DateTimeValidator(FieldName = "TillDate")]
    public DateTime TillDate { get; set; }
}
public class PushRouteResponseDto
{
    public int PushRegistrationId { get; set; }
    public int PushRouteId { get; set; }
    public DateTime CreatedDate { get; set; }
    public int State { get; set; }
    public IEnumerable<PushNotificationInfoDto> PushNotifications { get; set; }
}

public class PushNotificationInfoDto
{
    [Required]
    public int TrainNumber { get; set; }
    public DateTime DepartureTime { get; set; }    public DateTime ArrivalTime { get; set; }      [Required]
    public int DepartureStationId { get; set; }    [Required]
    public int ArrivalStationId { get; set; }      [Required]
    public int DepartutePlatform { get; set; }
    [Required]
    public int ArrivalPlatform { get; set; }
    [DateTimeValidator(FieldName = "TrainDate")]
    public DateTime TrainDate { get; set; }}

public class PushLogRequestDto
{
    public Nullable<int> PushRegistrationId { get; set; }
    [DateTimeValidator(FieldName = "FromDate")]
    public DateTime FromDate { get; set; }
    [DateTimeValidator(FieldName = "TillDate")]
    public DateTime TillDate { get; set; }
}

public class PushLogResponseDto
{
    public int Id { get; set; }
    public int PushRegistrationId { get; set; }
    public int PushRoutingId { get; set; }
    public DateTime CreatedDate { get; set; }
    public int TrainNumber { get; set; }
    public DateTime DepartureTime { get; set; }    public DateTime ArrivalTime { get; set; }      public int DepartureStationId { get; set; }    public int ArrivalStationId { get; set; }      public int DepartutePlatform { get; set; }
    public int ArrivalPlatform { get; set; }
}

public class TrainsDto
{
    [MaxLength(5120)]
    public string TrainNumber { get; set; }
}

public class StationIdsDto
{
    public string StationId { get; set; }
}

[AttributeUsage(AttributeTargets.Property)]
public class DateTimeValidatorAttribute : ValidationAttribute
{
    public string FieldName { get; set; }
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is not DateTime)
        {
            return new ValidationResult($"property [{ FieldName }] has an invalid date time format");
        }
        else if ((DateTime)value == DateTime.MinValue)
        {
            return new ValidationResult($"property [{ FieldName }] has an invalid date time value");
        }

        return ValidationResult.Success;
    }
}
