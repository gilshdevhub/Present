using API.Helpers.Validators;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Push;

public class PushNotificationsByDateDto
{
    [Required]
    public int PushRegistrationId { get; set; }
    public IEnumerable<PushNotificationInfoDto> PushNotifications { get; set; }
}

public class PushNotificationsByWeekDayDto
{
    [Required]
    public int PushRegistrationId { get; set; }
    public bool IsPermanentRegistration { get; set; }
    public IEnumerable<PushNotificationInfoByWeekDto> PushNotifications { get; set; }
}

public class PushNotificationInfoDto
{
    [Required]
    public int TrainNumber { get; set; }
    [Required]
    [DateTimeValidator(FieldName = "DepartureTime")]
    public DateTime DepartureTime { get; set; }    [Required]
    [DateTimeValidator(FieldName = "ArrivalTime")]
    public DateTime ArrivalTime { get; set; }      [Required]
    public int DepartureStationId { get; set; }    [Required]
    public int ArrivalStationId { get; set; }      [Required]
    public int DepartutePlatform { get; set; }
    [Required]
    public int ArrivalPlatform { get; set; }
    [Required]
    [DateTimeValidator(FieldName = "TrainDate")]
    public DateTime TrainDate { get; set; }}

public class PushNotificationInfoByWeekDto
{
    [Required]
    public int TrainNumber { get; set; }
    public DateTime CreatedDate { get; set; }      [Required]
    public int DepartureStationId { get; set; }    [Required]
    public int ArrivalStationId { get; set; }      [Required]
    public DateTime DepartureTime { get; set; }
    [Required]
    public DateTime ArrivalTime { get; set; }
    [Required]
    public int DepartutePlatform { get; set; }
    [Required]
    public int ArrivalPlatform { get; set; }
    public bool day1 { get; set; }
    public bool day2 { get; set; }
    public bool day3 { get; set; }
    public bool day4 { get; set; }
    public bool day5 { get; set; }
    public bool day6 { get; set; }
    public bool day7 { get; set; }
}

public class PushNotificationResponseDto
{
    public int PushRoutingId { get; set; }
}

public class PushNotificationUpdateRequestDto
{
    [Required]
    public int PushRoutingId { get; set; }
    public DaysDto WeekDays { get; set; }
}
public class DaysDto
{
    public bool day1 { get; set; }
    public bool day2 { get; set; }
    public bool day3 { get; set; }
    public bool day4 { get; set; }
    public bool day5 { get; set; }
    public bool day6 { get; set; }
    public bool day7 { get; set; }
}

public class PushDto
{
    public IEnumerable<PushByWeekDayDto> pushNotificationsByWeekDay { get; set; }
    public IEnumerable<PushByDateDto> pushNotificationsByDate { get; set; }
}

public class PushByDateDto
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public int TrainNumber { get; set; }
    public DateTime DepartureTime { get; set; }    public DateTime ArrivalTime { get; set; }      public int DepartureStationId { get; set; }    public int ArrivalStationId { get; set; }      public int DepartutePlatform { get; set; }
    public int ArrivalPlatform { get; set; }
    public DateTime TrainDate { get; set; }    public int PushRoutingId { get; set; }

}

public class PushByWeekDayDto
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public int TrainNumber { get; set; }
    public int DepartureStationId { get; set; }    public int ArrivalStationId { get; set; }      public int DepartutePlatform { get; set; }
    public int ArrivalPlatform { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public bool day1 { get; set; }
    public bool day2 { get; set; }
    public bool day3 { get; set; }
    public bool day4 { get; set; }
    public bool day5 { get; set; }
    public bool day6 { get; set; }
    public bool day7 { get; set; }
    public int PushRoutingId { get; set; }
}
