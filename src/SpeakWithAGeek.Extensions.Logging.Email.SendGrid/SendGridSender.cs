using System;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SpeakWithAGeek.Extensions.Logging.Email.SendGrid
{
    public class SendGridSender : IEmailSender
    {
        private readonly string _apiKey;
        public SendGridSender(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentException(nameof(apiKey));
            }
            _apiKey = apiKey;
        }
        public async Task SendAsync(EmailMessage email)
        {
            var client = new SendGridClient(_apiKey);

            var from = new EmailAddress(email.SenderEmail, email.SenderName);
            var to = new EmailAddress(email.RecipientEmail, email.RecipientName);

            var msg = MailHelper.CreateSingleEmail(from, to, email.Subject, string.Empty, email.Message);

            await client.SendEmailAsync(msg).ConfigureAwait(false);
        }
    }
}
