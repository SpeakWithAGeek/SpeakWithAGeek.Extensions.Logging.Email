using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SpeakWithAGeek.Extensions.Logging.Email
{
    public class EmailLogger : ILogger
    {
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly EmailLoggerOptions _configuration;
        private readonly string _name;
        private readonly IEmailSender _emailSender;

        public EmailLogger(string name, EmailLoggerOptions configuration, IEmailSender emailSender) : this(name, configuration, emailSender, null)
        {
        }

        public EmailLogger(string name, EmailLoggerOptions configuration, IEmailSender emailSender, Func<string, LogLevel, bool> filter)
        {
            _name = string.IsNullOrWhiteSpace(name) ? nameof(EmailLogger) : name;
            _filter = filter;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            if (string.IsNullOrWhiteSpace(_configuration.RecipientEmail))
            {
                throw  new Exception("Recipient email is missing");
            }

            if (string.IsNullOrWhiteSpace(_configuration.SenderEmail))
            {
                throw new Exception("Sender email is missing");
            }

            var message = BuildMessage(state, exception, formatter);

            var messageSubject = string.IsNullOrWhiteSpace(_configuration.EnvironmentName)
                ? $"[{logLevel.ToString()}] An error occured"
                : $"[{_configuration.EnvironmentName}] [{logLevel.ToString()}] An error occured";

            var email = new EmailMessage
            {
                SenderEmail = _configuration.SenderEmail,
                Message = message,
                RecipientName = _configuration.RecipientName,
                SenderName = _configuration.SenderName,
                Subject = messageSubject,
                RecipientEmail = _configuration.RecipientEmail 
            };

            Task.Run(
                async () =>
                {
                    await _emailSender.SendAsync(email).ConfigureAwait(false);
                }).ConfigureAwait(false);
        }

        private string BuildMessage<TState>(TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var messageBuilder = new StringBuilder();

            messageBuilder.Append($"<strong>Logger:</strong> {_name}</br>");
            messageBuilder.Append($"<strong>Message:</strong> {formatter(state, exception)}</br>");

            if (exception != null)
            {
                messageBuilder.Append("<strong>Exception details:</strong></br>");
                messageBuilder.Append($"<strong>Type:</strong> {exception.GetType()}</br>");
                messageBuilder.Append($"<strong>Message:</strong> {exception.Message}</br>");
                messageBuilder.Append($"<strong>Stack Trace: </strong> </br>{exception.StackTrace}");
            }

            return messageBuilder.ToString();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None && (_filter == null || _filter(_name, logLevel));
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new NoopDisposable();
        }

        private class NoopDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}
