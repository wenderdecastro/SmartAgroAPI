
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SmartAgroAPI.Services
{
    public class SMSService
    {

        private readonly IConfiguration _configuration;

        public SMSService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SendVerificationCode(string phoneNumber, string verificationCode)
        {
            TwilioClient.Init(_configuration["SMS:SmsId"], _configuration["SMS:SmsKey"]);

            MessageResource.Create(
                to: phoneNumber,
                from: _configuration["SMS:phoneNumber"],
                body: $"SmartAgro: Seu código de verificação é {verificationCode}");

        }
    }
}
