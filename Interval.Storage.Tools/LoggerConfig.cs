using log4net.Config;
using System;
using System.IO;

namespace Interval.Storage.Tools
{
    public sealed class LoggerConfig
    {
        
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
