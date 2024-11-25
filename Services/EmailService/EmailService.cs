
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace SmartAgroAPI.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings emailSettings;

        public EmailService(IOptions<SmtpSettings> options)
        {
            emailSettings = options.Value;

        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();

            email.Sender = MailboxAddress.Parse(emailSettings.Email);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();

            using (var smtp = new SmtpClient())
            {
                smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.Auto);

                smtp.Authenticate(emailSettings.Email, emailSettings.Password);

                await smtp.SendAsync(email);

            }
        }

    }
}
