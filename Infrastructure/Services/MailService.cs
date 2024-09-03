using Core.Config;
using Core.Entities.Mail;
using Core.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Services;

public class MailService : IMailService
{
    private readonly MailConfig _mailConfig;
    private readonly ILogger<MailService> _logger;

    public MailService(IOptions<MailConfig> mailConfig, ILogger<MailService> logger)
    {
        _mailConfig = mailConfig.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(MailRequest mailRequest)
    {
        MimeMessage email = new();

        email.Sender = MailboxAddress.Parse(_mailConfig.MailFrom);
        email.To.Add(MailboxAddress.Parse(_mailConfig.MailTo));
        email.Subject = mailRequest.Subject;
        email.Body = GetEmailBody(mailRequest);

        using SmtpClient smtp = new();

        try
        {
            await smtp.ConnectAsync(_mailConfig.Host, _mailConfig.Port).ConfigureAwait(false);
            await smtp.AuthenticateAsync(_mailConfig.MailFrom, _mailConfig.Password).ConfigureAwait(false);
            await smtp.SendAsync(email).ConfigureAwait(false);
            await smtp.DisconnectAsync(quit: true).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
                       _logger.LogError(ex, "send email failed");
        }
    }

    private static MimeEntity GetEmailBody(MailRequest mailRequest)
    {
        BodyBuilder builder = new();

        if (mailRequest.Attachments != null)
        {
            byte[] fileBytes;
            foreach (var file in mailRequest.Attachments)
            {
                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    _ = builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                }
            }
        }

        builder.TextBody = mailRequest.Body;
        return builder.ToMessageBody();
    }
}
