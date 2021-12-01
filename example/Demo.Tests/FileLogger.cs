using Demo.Specifics.Environment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Steps;

namespace Demo.Tests
{
    public sealed class FileLogger : ILogger
    {
        private readonly string _logFile;

        private readonly Dictionary<LogLevel, string> _prefixes = new Dictionary<LogLevel, string>
        {
            { LogLevel.Error, $"  [Error]: " },
            { LogLevel.Warning, $"[Warning]: " },
            { LogLevel.Info, $"   [Info]: " },
            { LogLevel.Debug, $"  [Debug]: \t" },
            { LogLevel.Trace, $"  [Trace]: \t\t" },
        };

        private readonly Dictionary<LogLevel, string> _consolePrefixes = new Dictionary<LogLevel, string>
        {
            { LogLevel.Error,   $"  ##[error]: " },
            { LogLevel.Warning, $"##[warning]: " },
            { LogLevel.Info,    $"     [Info]: " },
            { LogLevel.Debug,   $"    [Debug]:     " },
            { LogLevel.Trace,   $"    [Trace]:         " },
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogger"/> class.
        /// Logger writes all messages into text file.
        /// </summary>
        public FileLogger()
        {
            // Subscribe to step start event to log step.
            StepsEvents.OnStepStart += ReportStepInfo;

            var logsDirectory = Path.Combine(Config.Instance.TestsDir, "Logs"); ;

            if (!Directory.Exists(logsDirectory))
            {
                Directory.CreateDirectory(logsDirectory);
            }

            _logFile = Path.Combine(logsDirectory, "tests-execution.log");
        }

        /// <summary>
        /// Logs a message to a file with specified verbosity level and current timestamp.
        /// </summary>
        /// <param name="level">verbosity level</param>
        /// <param name="message">message to log</param>
        public void Log(LogLevel level, string message)
        {
            if (level <= Logger.Level)
            {
                var logString = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss.ff} {_prefixes[level]}{message}";
                WriteToFile(logString);

#if DEBUG
                System.Diagnostics.Debug.WriteLine(_consolePrefixes[level] + message);
#endif
            }
        }

        private void ReportStepInfo(MethodBase method, object[] arguments)
        {
            var info = StepsUtilities.GetStepInfo(method, arguments);
            Log(LogLevel.Info, "STEP: " + info);
        }

        /// <summary>
        /// Log info to the file
        /// </summary>
        /// <param name="text">text to log</param>
        private void WriteToFile(string text)
        {
            try
            {
                using (var file = new StreamWriter(_logFile, true))
                {
                    file.WriteLine(text);
                }
            }
            catch
            {
                // Just skip, if unable to write to file.
            }
        }
    }
}
