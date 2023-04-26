namespace aky.Foundation.Utility.Logging.Log4Net
{
    using System;
    using System.Reflection;
    using System.Xml;
    using aky.Foundation.Utility.Logging.Model;
    using log4net;
    using log4net.Repository;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public class Log4NetLogger : IakyLogger
    {
        private readonly string name;
        private readonly XmlElement xmlElement;
        private readonly ILog log;
        private ILoggerRepository loggerRepository;

        public Log4NetLogger(string name, XmlElement xmlElement)
        {
            this.name = name;
            this.xmlElement = xmlElement;
            this.loggerRepository = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            log = LogManager.GetLogger(this.loggerRepository.Name, name);
            log4net.Config.XmlConfigurator.Configure(this.loggerRepository, xmlElement);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return this.log.IsFatalEnabled;
                case LogLevel.Debug:
                case LogLevel.Trace:
                    return this.log.IsDebugEnabled;
                case LogLevel.Error:
                    return this.log.IsErrorEnabled;
                case LogLevel.Information:
                    return this.log.IsInfoEnabled;
                case LogLevel.Warning:
                    return this.log.IsWarnEnabled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            this.PerformLog(logLevel, eventId, state, exception, formatter);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter, ApplicationLog application)
        {
            this.PerformLog(logLevel, eventId, state, exception, formatter, application);
        }

        public void Log(string message, ApplicationLog application)
        {
            this.PerformLog(message, null, application);
        }

        public void LogError(string message, Exception exception, ApplicationLog application)
        {
            this.PerformLog(message, exception, application);
        }

        private void PerformLog(string message, Exception exception, ApplicationLog application)
        {
            if (application != null)
            {
                message = $"{message}, {"\r\n Custom data: "} {JsonConvert.SerializeObject(application)}";
            }

            this.log.Error(message, exception);
        }

        private void PerformLog<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter, ApplicationLog application = null)
        {
            if (!this.IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            string message = null;
            if (formatter != null)
            {
                message = formatter(state, exception);
            }

            if (application != null)
            {
                message = $"{message}, {"\r\n Custom data: "} {JsonConvert.SerializeObject(application)}";
            }

            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                switch (logLevel)
                {
                    case LogLevel.Critical:
                        this.log.Fatal(message);
                        break;
                    case LogLevel.Debug:
                    case LogLevel.Trace:
                        this.log.Debug(message);
                        break;
                    case LogLevel.Error:
                        this.log.Error(message, exception);
                        break;
                    case LogLevel.Information:
                        this.log.Info(message);
                        break;
                    case LogLevel.Warning:
                        this.log.Warn(message);
                        break;
                    default:
                        this.log.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                        this.log.Info(message, exception);
                        break;
                }
            }
        }
    }
}
