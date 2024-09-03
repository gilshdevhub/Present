namespace AccompanyingDisabled.Dtos;


public class DisabledTypeResponseDto
{
    public int Id { get; set; }
    public List<DisabledType> DisabledTypes { get; set; }
}

public class DisabledType
{
    public int Id { get; set; }
    public string Type { get; set; }
}