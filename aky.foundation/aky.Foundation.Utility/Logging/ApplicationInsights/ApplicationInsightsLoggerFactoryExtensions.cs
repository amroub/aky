namespace aky.Foundation.Utility.Logging.ApplicationInsights
{
    using System;
    using Microsoft.ApplicationInsights;
    using Microsoft.Extensions.Logging;

    public static class ApplicationInsightsLoggerFactoryExtensions
    {
        public static ILoggerFactory AddApplicationInsights(
        this ILoggerFactory factory,
        Func<string, LogLevel, bool> filter,
        ApplicationInsightsSettings settings)
        {
            factory.AddProvider(new ApplicationInsightsLoggerProvider(new TelemetryClient(), filter, settings));
            return factory;
        }

        public static ILoggerFactory AddApplicationInsights(
            this ILoggerFactory factory,
            ApplicationInsightsSettings settings)
        {
            factory.AddProvider(new ApplicationInsightsLoggerProvider(new TelemetryClient(), null, settings));

            return factory;
        }
    }
}
