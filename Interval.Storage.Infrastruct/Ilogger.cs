using System;

namespace Interval.Storage.Interface
{
    public interface Ilogger
    {
        void AddDebug(string message);
        
        void AddInformation(string message);

        void AddWarning(string message);

        void AddWaring(string message, Exception e);
        
        void AddError(string message);

        void AddError(string message, Exception e);

        bool IsDebugEnabled { get; }

        bool IsInfoEnabled { get; }

        bool IsWarnEnabled { get; } 

        bool IsErrorEnabled { get; } 

    }
}
