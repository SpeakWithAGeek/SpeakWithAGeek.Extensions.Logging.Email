using Microsoft.Extensions.Logging;
using System;

namespace SpeakWithAGeek.Extensions.Logging.Email
{
    [ProviderAlias("Email")]
    public class EmailLoggerProvider : ILoggerProvider
    {
        private readonly EmailLoggerOptions _configuration;
        private readonly IEmailSender _emailSender;
        private readonly Func<string, LogLevel, bool> _filter;

        public EmailLoggerProvider(EmailLoggerOptions configuration, IEmailSender emailSender)
        {
            _configuration = configuration;
            _emailSender = emailSender;
        }

        public EmailLoggerProvider(EmailLoggerOptions configuration)
        {
            _configuration = configuration;
        }

        public EmailLoggerProvider(EmailLoggerOptions configuration, IEmailSender emailSender, Func<string, LogLevel, bool> filter)
        {
            _configuration = configuration;
            _emailSender = emailSender;
            _filter = filter;
        }

        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new EmailLogger(categoryName, _configuration, _emailSender, _filter);
        }
    }
}
