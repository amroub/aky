namespace aky.Foundation.Utility.Logging.ApplicationInsights
{
    using System;
    using Microsoft.ApplicationInsights;
    using Microsoft.Extensions.Logging;

    public class ApplicationInsightsLoggerProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> filter;
        private readonly ApplicationInsightsSettings settings;
        private readonly TelemetryClient telemetryClient;

        public ApplicationInsightsLoggerProvider(TelemetryClient telemetryClient, Func<string, LogLevel, bool> filter, ApplicationInsightsSettings settings)
        {
            this.filter = filter;
            this.settings = settings;
            this.telemetryClient = telemetryClient;
        }

        public ILogger CreateLogger(string name)
        {
            return new ApplicationInsightsLogger(name, this.telemetryClient, this.filter, this.settings);
        }

        public void Dispose()
        {
        }
    }
}
