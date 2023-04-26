namespace Diatly.Foundation.Test.Diatly.Foundation.UtilityTest
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using global::Diatly.Foundation.Utility.Logging;
    using global::Diatly.Foundation.Utility.Logging.Model;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.Extensions.Logging;
    using Xunit;

    public class ApplicationInsightsLoggerTest : BaseFixture
    {
        private readonly IDiatlyLogger logger;

        public ApplicationInsightsLoggerTest()
        {
            this.logger = this.ApplicationInsightsLogger;
        }

        [Fact]
        public void Log4NetLogger_Log_LoggerIsAvailable()
        {
            Assert.NotNull(this.logger);
        }

        [Fact]
        public void Log4NetLogger_Log_SimpleLog()
        {
            string message = "Simple log";

            this.logger.LogInformation(message);
        }

        [Fact]
        public void Log4NetLogger_Log_WithCustomObject()
        {
            string message = "Simple log with custom object";

            this.logger.Log(
                message,
                new ApplicationLog()
                {
                    Application = "ApplicationInsightsLoggerTest",
                    ClaimsPrincipal = null,
                });
        }

        [Fact]
        public void Log4NetLogger_Log_WithCustomObjectBeingNull()
        {
            string message = "Simple log with custom object";

            this.logger.Log(message, null);
        }

        [Fact]
        public void Log4NetLogger_Log_WithException()
        {
            string message = "Error while performing mathematical evaluation";

            try
            {
                decimal a, b;
                a = 10;
                b = 0;
                decimal value = a / b;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, message);
            }
        }

        [Fact]
        public void Log4NetLogger_Log_WithExceptionWithCustomObject()
        {
            string message = "custom exception";

            try
            {
                throw new Exception("custom exception");
            }
            catch (Exception ex)
            {
                this.logger.LogError(
                message,
                ex,
                new ApplicationLog()
                {
                    Application = "ApplicationInsightsLoggerTest",
                    ClaimsPrincipal = null,
                });
            }
        }

        [Fact]
        public void Log4NetLogger_Log_WithExceptionWithCustomObjectBeingNull()
        {
            string message = "custom exception";

            try
            {
                throw new Exception("custom exception");
            }
            catch (Exception ex)
            {
                this.logger.LogError(message, ex, null);
            }
        }
    }
}
