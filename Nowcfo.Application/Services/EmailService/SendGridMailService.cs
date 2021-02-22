using Microsoft.Extensions.Configuration;
using Nowcfo.Application.Dtos.Email;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Nowcfo.Application.Services.EmailService
{

    public class SendGridMailService : IMailService
    {
        private readonly IConfiguration _configuration;
        public SendGridMailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(EmailDto emailDto)
        {
            var apiKey = _configuration["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("abhattarai@devfinity.io", "JWT Implementation");
            var to = new EmailAddress(emailDto.To);
            var msg = MailHelper.CreateSingleEmail(from,to,emailDto.Subject,emailDto.Body,emailDto.Body);
            var response = await client.SendEmailAsync(msg);
        }


    }
}
