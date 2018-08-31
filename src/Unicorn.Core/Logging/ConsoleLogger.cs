using System;

namespace Unicorn.Core.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Log(LogLevel level, string message)
        {
            string prefix = level.Equals(LogLevel.Debug) ? $"|\t\t" : string.Empty;
            System.Diagnostics.Debug.WriteLine($"{prefix}{level}: {message}");
        }
    }
}
