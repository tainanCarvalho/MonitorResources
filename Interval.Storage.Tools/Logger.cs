using Interval.Storage.Interface;
using log4net;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Interval.Storage.Tools
{
    [ExcludeFromCodeCoverage]
    public class Logger : Ilogger
    {
        private ILog log;
        public Logger(Type e) => log = LogManager.GetLogger(e);

        public bool IsInfoEnabled => log.IsInfoEnabled;

        public bool IsWarnEnabled => log.IsWarnEnabled;

        public bool IsErrorEnabled => log.IsErrorEnabled;

        public bool IsDebugEnabled => log.IsDebugEnabled;

        public void AddDebug(string message) => log.Debug(message); 

        public void AddError(string message) => log.Error(message);

        public void AddError(string message, Exception e) => log.Error(message, e);        

        public void AddInformation(string message) => log.Info(message);

        public void AddWaring(string message, Exception e) => log.Warn(message, e); 

        public void AddWarning(string message) => log.Warn(message);
    }
}
