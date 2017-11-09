using System.Threading.Tasks;

namespace SpeakWithAGeek.Extensions.Logging.Email
{
    public interface IEmailSender
    {
        Task SendAsync(EmailMessage email);
    }
}