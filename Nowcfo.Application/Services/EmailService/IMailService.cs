using Nowcfo.Application.Dtos.Email;
using System.Threading.Tasks;

namespace Nowcfo.Application.Services.EmailService
{
    public interface IMailService
    {
        Task SendEmailAsync(EmailDto emailDto);
    }
}