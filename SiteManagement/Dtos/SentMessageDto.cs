using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace SiteManagement.Dtos;

public class SentMessageCriteriaDto
{
    [Required]
    public DateTime SentFromDate { get; set; }
    [Required]
    public DateTime SentTillDate { get; set; }
    public MessageTypes MessageType { get; set; }
    public string? SearchInfo { get; set; }
}

public class SentMessageResponsetDto
{
    public int Id { get; set; }
    public int MessageType { get; set; }
    public string? MessageTypeInfo { get; set; }
    public string? Message { get; set; }
    public string? ResponseStatus { get; set; }
    public DateTime SentDate { get; set; }
    public int State { get; set; }
}

public class MessageInfoDto
{
    [Required]
    public MessageTypes MessageType { get; set; }
    [Required]
    [MaxLength(1024)]
    public string Message { get; set; }
    [MaxLength(8192)]
    public string[]? Keys { get; set; }
    [MaxLength(120)]
    public string? Subject { get; set; }// = string.Empty;
}
public class GetPushCountDto
{
    [Required]
    public MessageTypes MessageType { get; set; }
}
//public class StationsRequest
//{
//    [Required]
//    public int Id { get; set; }
//    [Required]
//    public DateTime RequestedDate { get; set; }
//}
