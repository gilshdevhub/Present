using Microsoft.AspNetCore.Http;

namespace Core.Entities.Mail;

public class MailRequest
{
    public string Subject { get; set; }
    public string Body { get; set; }
    public List<IFormFile> Attachments { get; set; }
}