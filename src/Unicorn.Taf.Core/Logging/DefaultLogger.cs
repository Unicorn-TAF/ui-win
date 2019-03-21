using System;
using System.Collections.Generic;
using Unicorn.Taf.Core.Testing.Tests;

namespace Unicorn.Taf.Core.Logging
{
    public class DefaultLogger : ILogger
    {
        private readonly Dictionary<LogLevel, string> prefixes = new Dictionary<LogLevel, string>
        {
            { LogLevel.Error, $"  [Error]: " },
            { LogLevel.Warning, $"[Warning]: " },
            { LogLevel.Info, $"   [Info]: " },
            { LogLevel.Debug, $"  [Debug]: \t" },
            { LogLevel.Trace, $"  [Trace]: \t\t" },
        };

        public void Log(LogLevel level, string message)
        {
            if (level <= Logger.Level)
            {
                var logString = $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff")} {prefixes[level]}{message}";
                SuiteMethod.LogOutput?.AppendLine(logString);
                Console.WriteLine(logString);
            }
        }
    }
}
