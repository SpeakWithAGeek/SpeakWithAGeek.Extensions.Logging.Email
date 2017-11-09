using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace SpeakWithAGeek.Extensions.Logging.Email
{
    public static class EmailLoggerFactoryExtentions
    {
        public static ILoggingBuilder AddEmail(this ILoggingBuilder builder, Func<IEmailSender> emailFactory, EmailLoggerOptions configuration)
        {
            builder.Services.AddSingleton<ILoggerProvider, EmailLoggerProvider>();
            builder.Services.AddSingleton(
                serviceProvider => emailFactory());
            builder.Services.AddSingleton(serviceProvider => configuration);
            return builder;
        }
        public static ILoggerFactory AddEmail(this ILoggerFactory factory, EmailLoggerOptions configuration, Func<IEmailSender> emailFactory)
        {
            return AddEmail(factory, configuration, emailFactory, LogLevel.Error);
        }

        public static ILoggerFactory AddEmail(this ILoggerFactory factory, EmailLoggerOptions configuration, Func<IEmailSender> emailFactory, Func<string, LogLevel, bool> filter)
        {
            factory.AddProvider(new EmailLoggerProvider(configuration, emailFactory(), filter));
            return factory;
        }

        public static ILoggerFactory AddEmail(this ILoggerFactory factory, EmailLoggerOptions configuration, Func<IEmailSender> emailFactory, LogLevel minLevel)
        {
            return AddEmail(
                factory,
                configuration,
                emailFactory,
                (_, logLevel) => logLevel >= minLevel);
        }
    }
}
