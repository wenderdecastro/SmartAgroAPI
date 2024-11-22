namespace SmartAgroAPI.Services.EmailService
{
    public class EmailSendingService
    {
        private readonly IEmailService emailService;
        public EmailSendingService(IEmailService service)
        {

            emailService = service;
        }

        public async Task SendRecoveryEmailAsync(string userName, string email, string codigo)
        {
            //try
            //{
            MailRequest request = new MailRequest
            {
                ToEmail = email,
                Subject = "Smart Agro - Recuperação de senha",
                Body = GetHtmlContentRecovery(userName, codigo)
            };

            await emailService.SendEmailAsync(request);
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        private string GetHtmlContentRecovery(string userName, string codigo)
        {
            string Response = $@"<!DOCTYPE html>
<div style=""width:100%; background-color:rgba(175, 232, 153, 1); padding: 20px;"">
    <div style=""max-width: 600px; margin: 0 auto; background-color:#FFFFFF; border-radius: 10px; padding: 20px;"">
        <p style=""color: #666666;text-align: center"">Olá, {userName}, Seu código de verificação é <b>{codigo}</b></p>
    </div>
</div>";

            return Response;
        }
    }
}

