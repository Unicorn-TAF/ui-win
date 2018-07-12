using System;
using Unicorn.Core.Logging;

namespace ProjectSpecific.Util
{
    public class ConsoleLogger : ILogger
    {
        public void Log(LogLevel level, string message)
        {
            string prefix = level.Equals(LogLevel.Debug) ? $"\t\t" : string.Empty;
            Console.WriteLine($"{prefix}{level}: {message}");
        }
    }
}
