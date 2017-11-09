using System;
using System.Net;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace SpeakWithAGeek.Extensions.Logging.Email.Smtp
{
    public class SmtpSender : IEmailSender
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _login;
        private readonly string _password;

        public SmtpSender(string host, int port, string login, string password)
        {
            _host = host;
            _port = port;
            _login = login;
            _password = password;
        }
        public async Task SendAsync(EmailMessage email)
        {
            var bb = new BodyBuilder
            {
                HtmlBody = email.Message
            };

            var message = new MimeMessage
            {
                Subject = email.Subject,
                Body = bb.ToMessageBody()
            };

            message.From.Add(new MailboxAddress(email.SenderName, email.SenderEmail));
            message.To.Add(new MailboxAddress(email.RecipientName, email.RecipientEmail));

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_host, _port).ConfigureAwait(false);

                var credentials = new NetworkCredential
                {
                    UserName = _login,
                    Password = _password
                };
                if (!string.IsNullOrEmpty(credentials.UserName) && !string.IsNullOrEmpty(credentials.Password))
                    await client.AuthenticateAsync(credentials).ConfigureAwait(false);

                await client.SendAsync(message).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }
    }
}
