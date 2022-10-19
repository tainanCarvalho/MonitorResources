using log4net.Config;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Interval.Storage.Tools
{
    public sealed class LoggerConfig
    {
        [ExcludeFromCodeCoverage]
        private LoggerConfig()
        {
            //private instance
        }

        [ExcludeFromCodeCoverage]
        public static void Config()
        {
#if DEBUG
            var logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogDebug.config");
#else
            var logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "\\Config\\Log4Net.config");
#endif
            FileInfo finfo = new FileInfo(logFilePath);
            XmlConfigurator.ConfigureAndWatch(finfo);
        }
    }
}
