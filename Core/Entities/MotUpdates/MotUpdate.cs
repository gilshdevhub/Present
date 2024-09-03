using System.ComponentModel.DataAnnotations;

namespace Core.Entities.MotUpdates;

public class MotUpdateRequest
{
    public string? LineRef { get; set; }
    public string MonitoringRef { get; set; }
}

public class MotUpdateResponse
{
    public Siri Siri { get; set; }
}
public class Siri
{
    public ServiceDelivery ServiceDelivery { get; set; }
}

public class ServiceDelivery
{
    public DateTimeOffset ResponseTimestamp { get; set; }
    public string ProducerRef { get; set; }
    public string ResponseMessageIdentifier { get; set; }
    public string RequestMessageRef { get; set; }
    public string Status { get; set; }
    public IEnumerable<StopMonitoringDelivery> StopMonitoringDelivery { get; set; }
    public ErrorCondition ErrorCondition { get; set; }
}

public class StopMonitoringDelivery
{
    public string version { get; set; }
    public DateTime ResponseTimestamp { get; set; }
    public string Status { get; set; }
    public IEnumerable<MonitoredStopVisit> MonitoredStopVisit { get; set; }
}

public class MonitoredStopVisit
{
    public DateTime RecordedAtTime { get; set; }
    public string ItemIdentifier { get; set; }
    public string MonitoringRef { get; set; }
    public MonitoredVehicleJourney MonitoredVehicleJourney { get; set; }
}
public class MonitoredVehicleJourney
{
    public string LineRef { get; set; }
    public string DirectionRef { get; set; }
    public FramedVehicleJourneyRef FramedVehicleJourneyRef { get; set; }
    public string PublishedLineName { get; set; }
    public string OperatorRef { get; set; }
    public string DestinationRef { get; set; }
    public DateTime OriginAimedDepartureTime { get; set; }
    public string ConfidenceLevel { get; set; }
    public VehicleLocation VehicleLocation { get; set; }
    public string Bearing { get; set; }
    public string Velocity { get; set; }
    public string VehicleRef { get; set; }
    public MonitoredCall MonitoredCall { get; set; }
}
public class FramedVehicleJourneyRef
{
    public string DataFrameRef { get; set; }
    public string DatedVehicleJourneyRef { get; set; }
}

public class VehicleLocation
{
    public string Longitude { get; set; }
    public string Latitude { get; set; }
}

public class MonitoredCall
{
    public string StopPointRef { get; set; }
    public string Order { get; set; }
    public DateTime ExpectedArrivalTime { get; set; }
    public string DistanceFromStop { get; set; }
    public DateTime AimedArrivalTime { get; set; }
}
public class ErrorCondition
{
    public OtherError OtherError { get; set; }
    public string Description { get; set; }
}

public class OtherError
{
    public string ErrorText { get; set; }
}
public class MotConvertion
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public int StationId { get; set; }
    [Required]
    public int MonitoringRef { get; set; }
    public string? BusStopLocation { get; set; }
}
public class PublicTransportStations
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


public class ResponseMotUpdatesByTrainStationDto
{
    public Object MotResponse { get; set; }
    public IEnumerable<PublicTransportStations> MotStations { get; set; }
}

public class BLSDto
{
    public Object MotResponse { get; set; }
    public IEnumerable<BusTripHeadSignsDto> BusTripHeadSigns { get; set; }
    public IEnumerable<NearBusStopsDto> NearBusStops { get; set; }
}

public class BusTripHeadSignsDto
{
    [Key]
    public int lineRef { get; set; }
    public string? first_stop_departure_time { get; set; }
    public DateOnly service_date { get; set; }
    public string? trip_headsign_final { get; set; }
    public string? trip_headsign_en_final { get; set; }
    public string? trip_headsign_ar_final { get; set; }
}

public class NearBusStopsDto
{
    public int monitoringRef { get; set; }
    public string busStopLocation { get; set; }
}

public class GTFSBLSDto
{
    public IEnumerable<BusStopTimesDto> BusStopTimes { get; set; }
    public IEnumerable<NearBusStopsDto> NearBusStops { get; set; }
}

public class BusStopTimesDto
{
    public int id { get; set; }
    public int monitoringRef { get; set; }
    public int stop_id { get; set; }
    public string trip_id { get; set; }
    public int route_id { get; set; }
    public int service_id { get; set; }
    public string? trip_headsign_final { get; set; }
    public string? trip_headsign_en_final { get; set; }
    public string? trip_headsign_ar_final { get; set; }
    public string publishedLineName { get; set; }
    public int operatorRef { get; set; }
    public int destinationRef { get; set; }
    public string expectedArrivalTime { get; set; }
    public string originAimedDepartureTime { get; set; }
    public DateTime service_date { get; set; }
}