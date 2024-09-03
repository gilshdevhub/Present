using Core.Entities.Messenger;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Mail;
using SmsSending.Dtos;
using System.Text;
using Ical.Net.Serialization;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;

namespace SmsSending.Controllers;

public class MailSendingController : BaseApiController
{
   // private readonly IMapper _mapper;
    private readonly IMessengerService _messengerService;
    private readonly IConfigurationService _configurationService;
    public MailSendingController( IMessengerService messengerService, IConfigurationService configurationService)//,IMapper mapper)
    {
        //_mapper = mapper;
        _messengerService = messengerService;
        _configurationService = configurationService;
    }
    [HttpPost("SendMail")]
    public async Task<ActionResult<SendGrid.Response>> MailSending([FromBody] MailInfoDto messageInfoDto)
    {
        if (messageInfoDto == null)
            return BadRequest(false);
        MailInfo messageInfo = new()
        {
            Addresses = messageInfoDto.Addresses,
            Message = messageInfoDto.Message,
            Subject = messageInfoDto.Subject
        };//JsonSerializer.Deserialize<MailInfo>(JsonSerializer.Serialize( messageInfoDto));
        string senderMailFrom = !(string.IsNullOrEmpty(messageInfoDto.SenderMail) || string.IsNullOrWhiteSpace(messageInfoDto.SenderMail)) ? messageInfoDto.SenderMail : "israelrailtender@gmail.com";

        var result = await _messengerService.MailSendingTender(messageInfo, senderMailFrom).ConfigureAwait(false);

        return Ok(result);
    }
    [HttpPost("SendAttachmentMail")]
    public async Task<ActionResult<SendGrid.Response>> MailSendingWithAttachment([FromBody] MailInfoAttachmentDto messageInfoDto)
    {
        if (messageInfoDto == null)
            return BadRequest(true);
        if (messageInfoDto.FileAttachment == null)
            return BadRequest(true);
        MailInfo messageInfo = new()
        {
            Addresses = messageInfoDto.Addresses,
            Message = messageInfoDto.Message,
            Subject = messageInfoDto.Subject
        };

        var result = await _messengerService.MailSendingAttachment(messageInfo, messageInfoDto.SenderMail, messageInfoDto.FileAttachment);
        return Ok(result);
    }
    [HttpPost("SendCalendarMail")]
    public async Task<ActionResult<SendGrid.Response>> MailSendingWithIcsAttachment([FromBody] MailInfoIcsAttachmentDto messageInfoDto)
    {
        var calendar = new Ical.Net.Calendar();
        var attendees = messageInfoDto.Addresses.Select(x => new Attendee()
        {
            Type = "INDIVIDUAL",
            CommonName = x.Name,
            Role = "TENTATIVE",
            ParticipationStatus = "TENTATIVE",
            Rsvp = true,
            Value = new Uri($"mailto:{x.Email}"),
        }).ToList();
        var al = new Alarm()
        {
            Duration = TimeSpan.FromSeconds(10),
            Action = "DISPLAY",
            Repeat = 1
        };
        TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time");
        string senderMailFrom = !(string.IsNullOrEmpty(messageInfoDto.SenderMail) || string.IsNullOrWhiteSpace(messageInfoDto.SenderMail)) ? messageInfoDto.SenderMail : "israelrailtender@gmail.com";
        var e = new CalendarEvent
        {
            Summary = "Israel Railways Reminder",
            IsAllDay = false,
            Organizer = new Organizer()
            {
                CommonName = "Israel Railways",
                Value =  new Uri($"mailto:{senderMailFrom}")
            },
            Attendees = attendees,
            Start = new CalDateTime(messageInfoDto.StartTime.Value.ToUniversalTime()),
            End = new CalDateTime(messageInfoDto.EndTime.Value.ToUniversalTime()),
            DtStamp = new CalDateTime(DateTime.Now.ToUniversalTime()),
            Status = "CONFIRMED",
            Alarms = { al }
        };
        calendar.Events.Add(e);
        calendar.Method = "REQUEST";
        var serializer = new CalendarSerializer();
        var serializedCalendar = serializer.SerializeToString(calendar);
        var bytesCalendar = Encoding.ASCII.GetBytes(serializedCalendar);
        MemoryStream ms = new (bytesCalendar);
        System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(ms, "IsraelRailwaysInvite.ics", "text/calendar");
        var messageEmail = new SendGridMessage();
        messageEmail.AddAttachment("IsraelRailwaysInvite.ics", Convert.ToBase64String(bytesCalendar));
        SendGrid.Helpers.Mail.Attachment attachmentIcs = messageEmail.Attachments.First();
         MailInfo messageInfo = new()
        {
            Addresses = messageInfoDto.Addresses,
            Message = messageInfoDto.Message,
            Subject = messageInfoDto.Subject
        };
        var result = await _messengerService.MailSendingAttachment(messageInfo, messageInfoDto.SenderMail, attachmentIcs);

        return result;
    }

}
