using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.ReportPortalAgent;
using Unicorn.Taf.Core.Utility;
using System.IO;
using System.Drawing.Imaging;
using Demo.Specifics.Environment;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing;
using System.Collections.Generic;
using System;

namespace Demo.Tests
{
    [TestAssembly]
    public class TestsAssembly
    {
        private static ReportPortalReporterInstance reporter;
        private static Screenshotter screenshotter;

        [RunInitialize]
        public static void InitRun()
        {
            Logger.Instance = new TextFileLogger();
            Logger.Level = LogLevel.Trace;
            var screenshotsDir = Path.Combine(Config.Instance.TestsDir, "Screenshots");
            screenshotter = new Screenshotter(screenshotsDir, ImageFormat.Png, true);
            reporter = new ReportPortalReporterInstance();
        }

        [RunFinalize]
        public static void FinalizeRun()
        {
            reporter.Dispose();
            screenshotter.Unsubscribe();
            reporter = null;
            screenshotter = null;
        }
    }

    /// <summary>
    /// Describes logger to text file.
    /// </summary>
    public sealed class TextFileLogger : ILogger
    {
        private readonly string _commonLog;
        private readonly string _logsDirectory;

        private bool consoleOutput;

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

        private string currentFileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFileLogger"/> class.
        /// Logger writes all messages into suite-tied text file or common file for out-of-suite activities.
        /// </summary>
        public TextFileLogger()
        {
            _logsDirectory = "D:\\Logs";

            if (!Directory.Exists(_logsDirectory))
            {
                Directory.CreateDirectory(_logsDirectory);
            }

            currentFileName = Path.Combine(_logsDirectory, "log.log");
            consoleOutput = true;
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

                if (consoleOutput)
                {
                    Console.WriteLine(_consolePrefixes[level] + message);
                }
            }
        }

        private void StartSuiteLog(TestSuite suite)
        {
            consoleOutput = false;
        }

        /// <summary>
        /// Log info to the file
        /// </summary>
        /// <param name="text">text to log</param>
        private void WriteToFile(string text)
        {
            try
            {
                using (var file = new StreamWriter(currentFileName, true))
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
