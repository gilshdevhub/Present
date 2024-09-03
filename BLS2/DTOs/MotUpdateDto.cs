using System.ComponentModel.DataAnnotations;

namespace BLS2.Dtos;


public class MotUpdateResponseDto
{
    public SiriDto Siri { get; set; }
}
public class SiriDto
{
    public ServiceDeliveryDto ServiceDelivery { get; set; }
}

public class ServiceDeliveryDto
{
    public DateTimeOffset ResponseTimestamp { get; set; }
    public string ProducerRef { get; set; }
    public string ResponseMessageIdentifier { get; set; }
    public string RequestMessageRef { get; set; }
    public string Status { get; set; }
    public IEnumerable<StopMonitoringDeliveryDto> StopMonitoringDelivery { get; set; }
    public ErrorConditionDto ErrorCondition { get; set; }
}

public class StopMonitoringDeliveryDto
{
    public string version { get; set; }
    public DateTime ResponseTimestamp { get; set; }
    public string Status { get; set; }
    public IEnumerable<MonitoredStopVisitDto> MonitoredStopVisit { get; set; }
}

public class MonitoredStopVisitDto
{
    public DateTime RecordedAtTime { get; set; }
    public string ItemIdentifier { get; set; }
    public string MonitoringRef { get; set; }
    public MonitoredVehicleJourneyDto MonitoredVehicleJourney { get; set; }
}
public class MonitoredVehicleJourneyDto
{
    public string LineRef { get; set; }
    public string DirectionRef { get; set; }
    public FramedVehicleJourneyRefDto FramedVehicleJourneyRef { get; set; }
    public string PublishedLineName { get; set; }
    public string OperatorRef { get; set; }
    public string DestinationRef { get; set; }
    public DateTime OriginAimedDepartureTime { get; set; }
    public string ConfidenceLevel { get; set; }
    public VehicleLocationDto VehicleLocation { get; set; }
    public string Bearing { get; set; }
    public string Velocity { get; set; }
    public string VehicleRef { get; set; }
    public MonitoredCallDto MonitoredCall { get; set; }
}
public class FramedVehicleJourneyRefDto
{
    public string DataFrameRef { get; set; }
    public string DatedVehicleJourneyRef { get; set; }
}

public class VehicleLocationDto
{
    public string Longitude { get; set; }
    public string Latitude { get; set; }
}

public class MonitoredCallDto
{
    public string StopPointRef { get; set; }
    public string Order { get; set; }
    public DateTime ExpectedArrivalTime { get; set; }
    public string DistanceFromStop { get; set; }
    public DateTime AimedArrivalTime { get; set; }
}
public class ErrorConditionDto
{
    public OtherErrorDto OtherError { get; set; }
    public string Description { get; set; }
}

public class OtherErrorDto
{
    public string ErrorText { get; set; }
}
public class PublicTransportStationsDto
{
    public int Uid { get; set; }
    [Key]
    public int Id { get; set; }
    public int? Code { get; set; }
    [MaxLength(500)]
    public string? NameHe { get; set; }
    [MaxLength(500)]
    public string? NameAr { get; set; }
    [MaxLength(500)]
    public string? NameEn { get; set; }
    [MaxLength(500)]
    public string? NameRu { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int? Type_id { get; set; }
    [MaxLength(100)]
    public string? City { get; set; }
    [MaxLength(1500)]
    public string? LocationDescriptionHe { get; set; }
    [MaxLength(1500)]
    public string? LocationDescriptionEn { get; set; }
    [MaxLength(1500)]
    public string? LocationDescriptionAr { get; set; }
    [MaxLength(1500)]
    public string? LocationDescriptionRu { get; set; }
    [MaxLength(500)]
    public string? Stop_name { get; set; }
}

