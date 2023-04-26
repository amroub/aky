namespace aky.Foundation.Utility.Logging
{
    using System;
    using aky.Foundation.Utility.Logging.Model;
    using Microsoft.Extensions.Logging;

    public interface IakyLogger : ILogger
    {
        void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter, ApplicationLog application);

        void Log(string message, ApplicationLog applicationLog);

        void LogError(string message, Exception exception, ApplicationLog applicationLog);
    }
}
