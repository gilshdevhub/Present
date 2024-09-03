using Core.Enums;

namespace Core.Entities.TimeTable;

public class TrainTimeTableRequest
{
    public int SysytemId { get; set; } = 1;
    public string SystemUserName { get; set; } = "1";
    public string SystemPass { get; set; } = "1";
    public int FromStation { get; set; }
    public int ToStation { get; set; }
    public string Date { get; set; }
    public TimeSpan Hours { get; set; }
    public SystemTypes SystemType { get; set; }
    public ScheduleTypes ScheduleType { get; set; }
    public Languages LanguageId { get; set; }
}

public class TrainTimeTableByTrainNumberRequest : TrainTimeTableRequest
{
    public int TrainNUmber { get; set; }
}

public class TrainTimeTableRespnse
{
    public int NumOfResultsToShow { get; set; }
    public int StartFromIndex { get; set; }
    public int OnFocusIndex { get; set; }
    public int ClientMessageId { get; set; } = 1;
    public bool FreeSeatsError { get; set; } = false;
    public IEnumerable<Travel> Travels { get; set; }
}
