using Core.Entities.Mail;

namespace Core.Interfaces;

public interface IMailService
{
    Task SendEmailAsync(MailRequest mailRequest);
}
