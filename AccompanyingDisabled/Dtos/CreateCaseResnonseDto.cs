namespace AccompanyingDisabled.Dtos;

public class CreateCaseResnonseDto
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public int ErrorCode { get; set; }
    public string InicidendId { get; set; }
    public string? CaseNumber { get; set; }
}