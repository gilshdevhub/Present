using Core.Enums;
using Core.Helpers.Validators;
using SendGrid.Helpers.Mail;
using System.ComponentModel.DataAnnotations;

namespace SmsSending.Dtos
{
    public class MessageInfoDto
    {
        [Required]
        [MaxLength(1024)]
        public string message { get; set; }
        [Required]
        public string[] numbers { get; set; }
        [Required]
        [EnumRangeValidator]
        public SystemTypes systemType { get; set; }
        [Required]
        [EnumRangeValidator]
        public SmsSubscriberType subscriber { get; set; }
        [Required]
        public int msgQ { get; set; }
    }
    public class MailInfoDto
    {
        public string? SenderMail { get; set; }
        public string Message { get; set; }
        public List<EmailAddress> Addresses { get; set; }
        public string Subject { get; set; }
    }
    public class MailInfoAttachmentDto
    {
        public string? SenderMail { get; set; }
        public string Message { get; set; }
        public List<EmailAddress> Addresses { get; set; }
        public string Subject { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public SendGrid.Helpers.Mail.Attachment? FileAttachment { get; set; }
    }
    
        public class MailInfoIcsAttachmentDto
    {
        public string? SenderMail { get; set; }
        public string Message { get; set; }
        public List<EmailAddress> Addresses { get; set; }
        public string Subject { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
