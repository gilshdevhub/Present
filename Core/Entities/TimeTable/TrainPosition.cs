namespace Core.Entities.TimeTable;

public class TrainPosition
{
    public int CurrentLastStation { get; set; }
    public int NextStation { get; set; }
    public int CalcDiffMinutes { get; set; }
}
