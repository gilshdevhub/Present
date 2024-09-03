namespace AccompanyingDisabled.Dtos;

public class AttachFilesToCaseRequestDto
{
    public string CaseNumber { get; set; }
    public string Token { get; set; }
    public FileRequestDto[] Files { get; set; }
}