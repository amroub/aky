namespace aky.Foundation.Utility.Logging.Log4Net
{
    using System.Collections.Concurrent;
    using System.IO;
    using System.Xml;
    using Microsoft.Extensions.Logging;

    public class Log4NetProvider : ILoggerProvider
    {
        private readonly string _log4NetConfigFile;
        private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers = new ConcurrentDictionary<string, Log4NetLogger>();

        public Log4NetProvider(string log4NetConfigFile)
        {
            this._log4NetConfigFile = log4NetConfigFile;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return this._loggers.GetOrAdd(categoryName, this.CreateLoggerImplementation);
        }

        public void Dispose()
        {
            this._loggers.Clear();
        }

        private static XmlElement Parselog4NetConfigFile(string filename)
        {
            XmlDocument log4netConfig = new XmlDocument();

            using (var fileConfig = File.OpenRead(filename))
            {
                log4netConfig.Load(fileConfig);
                return log4netConfig["log4net"];
            }
        }

        private Log4NetLogger CreateLoggerImplementation(string name)
        {
            return new Log4NetLogger(name, Parselog4NetConfigFile(this._log4NetConfigFile));
        }
    }
}
