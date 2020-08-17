using System;
using System.Collections.Generic;
using System.Globalization;

namespace Unicorn.Taf.Core.Logging
{
    /// <summary>
    /// Provides default implementation of framework logger (used if no other loggers were assigned).
    /// Output: console.
    /// </summary>
    public class DefaultLogger : ILogger
    {
        private readonly Dictionary<LogLevel, string> _prefixes = new Dictionary<LogLevel, string>
        {
            { LogLevel.Error, $"  [Error]: " },
            { LogLevel.Warning, $"[Warning]: " },
            { LogLevel.Info, $"   [Info]: " },
            { LogLevel.Debug, $"  [Debug]: \t" },
            { LogLevel.Trace, $"  [Trace]: \t\t" },
        };

        /// <summary>
        /// Log message with specified severity with current timestamp.
        /// </summary>
        /// <param name="level"><see cref="LogLevel"/> severity level</param>
        /// <param name="message">message to log</param>
        public void Log(LogLevel level, string message)
        {
            if (level <= Logger.Level)
            {
                var logString = $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff", CultureInfo.InvariantCulture)} {_prefixes[level]}{message}";
                Console.WriteLine(logString);
            }
        }
    }
}
