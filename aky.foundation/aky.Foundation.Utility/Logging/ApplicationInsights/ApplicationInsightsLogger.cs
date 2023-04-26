namespace aky.Foundation.Utility.Logging.ApplicationInsights
{
    using System;
    using aky.Foundation.Utility.Logging.Model;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.Extensions.Logging;
    using aky.Foundation.Utility;
    using System.Collections.Generic;

    public class ApplicationInsightsLogger : IakyLogger
    {
        private readonly string name;
        private readonly Func<string, LogLevel, bool> filter;
        private readonly ApplicationInsightsSettings settings;
        private readonly TelemetryClient telemetryClient;

        public ApplicationInsightsLogger(string name, TelemetryClient telemetryClient)
            : this(name, telemetryClient, null, new ApplicationInsightsSettings())
        {
        }

        public ApplicationInsightsLogger(string name, TelemetryClient telemetryClient, Func<string, LogLevel, bool> filter, ApplicationInsightsSettings settings)
        {
            this.name = string.IsNullOrEmpty(name) ? nameof(ApplicationInsightsLogger) : name;
            this.filter = filter;
            this.settings = settings;
            this.telemetryClient = telemetryClient;

            if (this.settings.DeveloperMode.HasValue)
            {
                TelemetryConfiguration.Active.TelemetryChannel.DeveloperMode = this.settings.DeveloperMode;
            }

            if (!this.settings.DeveloperMode.Value)
            {
                if (string.IsNullOrWhiteSpace(this.settings.InstrumentationKey))
                {
                    throw new ArgumentNullException(nameof(ApplicationInsightsLogger.settings.InstrumentationKey));
                }

                TelemetryConfiguration.Active.InstrumentationKey = this.settings.InstrumentationKey;
                this.telemetryClient.InstrumentationKey = this.settings.InstrumentationKey;
            }
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return this.filter == null || this.filter(this.name, logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            this.PerformLog(logLevel, eventId, state, exception, formatter, null);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter, ApplicationLog applicationLog)
        {
            this.PerformLog(logLevel, eventId, state, exception, formatter, applicationLog);
        }

        public void Log(string message, ApplicationLog applicationLog)
        {
            if (!this.IsEnabled(LogLevel.Information))
            {
                return;
            }

            this.PerformLog(message, null, applicationLog);
        }

        public void LogError(string message, Exception exception, ApplicationLog applicationLog)
        {
            if (!this.IsEnabled(LogLevel.Error))
            {
                return;
            }

            this.PerformLog(message, null, applicationLog);
        }

        private void PerformLog(string message, Exception exception, ApplicationLog applicationLog)
        {
            IDictionary<string, string> properties = null;

            if (applicationLog != null)
            {
                properties = applicationLog.ToDictionary<string>();
            }

            if (exception != null)
            {
                ExceptionTelemetry exceptionTelemetry = new ExceptionTelemetry(exception);
                exceptionTelemetry.Message = message;

                if (properties != null)
                {
                    foreach (var prop in properties)
                    {
                        exceptionTelemetry.Properties.Add(prop);
                    }
                }

                this.telemetryClient.TrackException(exceptionTelemetry);
            }
            else
            {
                if (message == null)
                {
                    return;
                }

                TraceTelemetry traceTelemetry = new TraceTelemetry(message);

                if (properties != null)
                {
                    foreach (var prop in properties)
                    {
                        traceTelemetry.Properties.Add(prop);
                    }
                }

                this.telemetryClient.TrackTrace(traceTelemetry);
            }
        }

        private void PerformLog<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter, ApplicationLog applicationLog)
        {
            if (!this.IsEnabled(logLevel))
            {
                return;
            }

            if (exception != null)
            {
                this.telemetryClient.TrackException(new ExceptionTelemetry(exception));
                return;
            }

            var message = string.Empty;
            if (formatter != null)
            {
                message = formatter(state, exception);
            }
            else
            {
                if (state != null)
                {
                    message += state;
                }
            }

            if (!string.IsNullOrEmpty(message))
            {
                this.telemetryClient.TrackTrace(message, GetSeverityLevel(logLevel));
            }
        }

        private static SeverityLevel GetSeverityLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical: return SeverityLevel.Critical;
                case LogLevel.Error: return SeverityLevel.Error;
                case LogLevel.Warning: return SeverityLevel.Warning;
                case LogLevel.Information: return SeverityLevel.Information;
                case LogLevel.Trace:
                default: return SeverityLevel.Verbose;
            }
        }
    }
}
