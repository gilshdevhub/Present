using System.ComponentModel.DataAnnotations;

namespace AccompanyingDisabled.Dtos;

public class GetEscortLuzResponseDto
{
    [Required]
    [Range(1, 2)]
    public int RequestStatus { get; set; } = 0;
    [Required]
    public string StatusDiscription { get; set; } = "יש תקלה במערכת חייג לשירות הלקוחות *2560";
    [Required]
    public List<EscortData> EscortsData { get; set; }
}

public class EscortData
{
    [Required]
    public int UserId { get; set; }
    [Required]
    public string Subject { get; set; }
    [Required]
    public string Body { get; set; }
    [Required]
    public int TrainNum { get; set; }
    [Required]
    public DateTime TrainDate { get; set; }
    [Required]
    public string MessageGuid { get; set; }
    [Required]
    public int StatusId { get; set; }
}
