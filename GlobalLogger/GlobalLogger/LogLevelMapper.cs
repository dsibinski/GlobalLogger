using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlobalLogger.Exceptions;
using NLog;

namespace GlobalLogger
{
    internal class LogLevelMapper
    {
        internal static LogLevel GetNlogLevel(Level level)
        {
            switch (level)
            {
                case Level.Trace:
                    return LogLevel.Trace;
                case Level.Debug:
                    return LogLevel.Debug;
                case Level.Info:
                    return LogLevel.Info;
                case Level.Warning:
                    return LogLevel.Warn;
                case Level.Error:
                    return LogLevel.Error;
                case Level.Fatal:
                    return LogLevel.Fatal;
                default:
                    throw new GlobalLoggerException($"Unknown log Level {level}");
            }
        }
    }
}
