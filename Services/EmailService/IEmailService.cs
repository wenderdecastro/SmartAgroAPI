namespace SmartAgroAPI.Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
