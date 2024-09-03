using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class RailTimeTableRequest
    {
        [Required]
        [Range(minimum: 1, maximum: 9999)]
        public int FromStation { get; set; }
        [Required]
        [Range(minimum: 1, maximum: 9999)]
        public int ToStation { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Hour { get; set; }
        [Required]
        [Range(minimum: 1, maximum: 2)]
        public SystemTypes SystemType { get; set; }
        [Required]
        public ScheduleTypes ScheduleType { get; set; }
    }

    public class RailTimeTableResponse
    {
        public int Seats { get; set; }
        public int NumOfResultsToShow { get; set; }
        public IEnumerable<TrainDetail> Trains { get; set; }
    }

    public class TrainDetail
    {
        public int TrainNumber { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public int Crowed { get; set; }
        public int OriginStation { get; set; }
        public int DestinationStation { get; set; }
        public int OriginPlatform { get; set; }
        public int DestinationPlatform { get; set; }
        public bool Handicap { get; set; }
        public TrainPosision TrainPosision { get; set; }
        public IEnumerable<StopStation> StopStations { get; set; }
        public IEnumerable<RouteStation> RouteStations { get; set; }
    }

    public class TrainPosision
    {
        public int CalcDiffMinutes { get; set; }
        public int CurrentLastStation { get; set; }
        public int NextStation { get; set; }
    }

    public class StopStation
    {
        public int StationId { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DpartureTime { get; set; }
        public int Croweded { get; set; }
        public int ReserveSeats { get; set; }
        public int Platrform { get; set; }
    }

    public class RouteStation
    {
        public int StationId { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int Croweded { get; set; }
        public int ReserveSeats { get; set; }
        public int Platrform { get; set; }
    }
}
