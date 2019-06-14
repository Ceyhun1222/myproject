using System;

namespace PVT.Engine.Common.Logging
{
    public interface ILogger
    {
        
        void Error(string message, object argument);
        void Error(Exception ex, string message);
        void Fatal(string message, object argument);
        void Warn(string message, object argument);
        void Info(string message, object argument);
        void Debug(string message, object argument);
        void Trace(string message, object argument);
    }
}
