using Core.Application.Dtos.Email;
using Core.Application.Interfaces.Services;
using Core.Domain.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Security;


namespace Infrastructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        //we place our getter instead of setter
        private MailSettings _mailSettings { get; }

        //This is used to be able to inject the settings in our class
        public EmailService (IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendAsync(EmailRequest emailRequest)
        {
            try
            {
                //Creating email
                MimeMessage email = new();

                email.Sender = MailboxAddress.Parse($"{_mailSettings.DisplayName} + < {_mailSettings.EmailFrom} >");
                email.To.Add(MailboxAddress.Parse(emailRequest.To));
                email.Subject= emailRequest.Subject;

                //Creating bodyHtmlAllowance
                BodyBuilder builder = new();
                builder.HtmlBody = emailRequest.Body;

                email.Body = builder.ToMessageBody();

                using SmtpClient smtp = new();
                smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);

                //Sending email
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

            } 
            catch(Exception ex)
            {

            }
        }
    }
}
