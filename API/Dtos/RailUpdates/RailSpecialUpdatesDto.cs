namespace API.Dtos.RailUpdates;

public class RailSpecialUpdatesRequestDto : RailUpdateRequestDto
{
    public int OriginStationId { get; set; }
    public int TargetStationId { get; set; }
}

public class RailSpecialUpdatesResponseDto : RailUpdateResponseDto
{
}
