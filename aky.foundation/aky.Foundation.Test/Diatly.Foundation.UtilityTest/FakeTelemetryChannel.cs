namespace Diatly.Foundation.Test.Diatly.Foundation.UtilityTest
{
    using System.Collections.Concurrent;
    using Microsoft.ApplicationInsights.Channel;

    public class FakeTelemetryChannel : ITelemetryChannel
    {
        public ConcurrentBag<ITelemetry> SentTelemtries = new ConcurrentBag<ITelemetry>();

        public bool IsFlushed { get; private set; }

        public bool? DeveloperMode { get; set; }

        public string EndpointAddress { get; set; }

        public void Send(ITelemetry item)
        {
            SentTelemtries.Add(item);
        }

        public void Flush()
        {
            IsFlushed = true;
        }

        public void Dispose()
        {

        }
    }
}
