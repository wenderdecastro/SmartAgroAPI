
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace SmartAgroAPI.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            emailSettings = options.Value;

        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();

            email.Sender = MailboxAddress.Parse(Environment.GetEnvironmentVariable("EMAILSETTINGS_EMAILSENDER"));
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();

            using (var smtp = new SmtpClient())
            {
                smtp.Connect(Environment.GetEnvironmentVariable("EMAILSETTINGS_HOST"), Convert.ToInt32(Environment.GetEnvironmentVariable("EMAILSETTINGS_PORT")), SecureSocketOptions.StartTls);

                smtp.Authenticate(Environment.GetEnvironmentVariable("EMAILSETTINGS_EMAILSENDER"), Environment.GetEnvironmentVariable("EMAILSETTINGS_APPPASSWORD"));

                await smtp.SendAsync(email);

            }
        }

    }
}
