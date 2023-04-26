namespace Diatly.Foundation.Test.Diatly.Foundation.UtilityTest
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Xml;
    using global::Diatly.Foundation.Utility.Logging;
    using global::Diatly.Foundation.Utility.Logging.Model;
    using Microsoft.Extensions.Logging;
    using Xunit;

    public class Log4NetLoggerTest : BaseFixture
    {
        private readonly IDiatlyLogger logger;
        private const string LogFileName = "app.log";

        public Log4NetLoggerTest()
        {
            this.logger = this.Log4NetLogger;
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

            string logContent = this.ReadFile(LogFileName);

            Assert.Contains(message, logContent, StringComparison.InvariantCulture);
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

            string logContent = this.ReadFile(LogFileName);

            Assert.Contains(message, logContent, StringComparison.InvariantCulture);
        }

        [Fact]
        public void Log4NetLogger_Log_WithCustomObjectBeingNull()
        {
            string message = "Simple log with custom object";

            this.logger.Log(message, null);

            string logContent = this.ReadFile(LogFileName);

            Assert.Contains(message, logContent, StringComparison.InvariantCulture);
        }

        [Fact]
        public void Log4NetLogger_Log_WithException()
        {
            try
            {
                decimal a, b;
                a = 10;
                b = 0;
                decimal value = a / b;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "DivideByZeroException");
            }

            string logContent = this.ReadFile(LogFileName);

            Assert.Contains("DivideByZeroException", logContent, StringComparison.InvariantCulture);
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

            string logContent = this.ReadFile(LogFileName);

            Assert.Contains(message, logContent, StringComparison.InvariantCulture);
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

            string logContent = this.ReadFile(LogFileName);

            Assert.Contains(message, logContent, StringComparison.InvariantCulture);
        }

        private string ToApplicationPath(string fileName)
        {
            var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*");
            var appRoot = appPathMatcher.Match(exePath).Value;
            return Path.Combine(appRoot, fileName);
        }

        private string ReadFile(string file)
        {
            string logContent = string.Empty;

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs, Encoding.Default))
            {
                logContent = sr.ReadToEnd();
            }

            return logContent;
        }
    }
}
