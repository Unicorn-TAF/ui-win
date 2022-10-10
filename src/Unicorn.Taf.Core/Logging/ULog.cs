namespace Unicorn.Taf.Core.Logging
{
    /// <summary>
    /// Main framework logger.
    /// </summary>
    public static class ULog
    {
        private static LogLevel Level = LogLevel.Debug;
        private static Api.ILogger Instance = null;

        /// <summary>
        /// Sets active logger implementation.
        /// </summary>
        /// <param name="logger">logger instance</param>
        public static void SetLogger(Api.ILogger logger) =>
            Instance = logger;

        /// <summary>
        /// Sets minimum log level. All records with level lower than current will not appear in logs.
        /// </summary>
        /// <param name="level">verbosity level</param>
        public static void SetLevel(LogLevel level)
        {
            Level = level;
            Logger.Level = level;
        }

        /// <summary>
        /// Logs message with error level.
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="parameters">parameters to substitute to message template</param>
        public static void Error(string message, params object[] parameters)
        {
            if (Instance is null)
            {
                Logger.Instance.Log(LogLevel.Error, string.Format(message, parameters));
            }
            else
            {
                Instance.Error(message, parameters);
            }
        }

        /// <summary>
        /// Logs message with warning level.
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="parameters">parameters to substitute to message template</param>
        public static void Warn(string message, params object[] parameters)
        {
            if (Level < LogLevel.Warning)
            {
                return;
            }

            if (Instance is null)
            {
                Logger.Instance.Log(LogLevel.Warning, string.Format(message, parameters));
            }
            else
            {
                Instance.Warn(message, parameters);
            }
        }

        /// <summary>
        /// Logs message with informational level.
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="parameters">parameters to substitute to message template</param>
        public static void Info(string message, params object[] parameters)
        {
            if (Level < LogLevel.Info)
            {
                return;
            }

            if (Instance is null)
            {
                Logger.Instance.Log(LogLevel.Info, string.Format(message, parameters));
            }
            else
            {
                Instance.Info(message, parameters);
            }
        }

        /// <summary>
        /// Logs message with debug level.
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="parameters">parameters to substitute to message template</param>
        public static void Debug(string message, params object[] parameters)
        {
            if (Level < LogLevel.Debug)
            {
                return;
            }

            if (Instance is null)
            {
                Logger.Instance.Log(LogLevel.Debug, string.Format(message, parameters));
            }
            else
            {
                Instance.Debug(message, parameters);
            }
        }

        /// <summary>
        /// Logs message with trace level.
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="parameters">parameters to substitute to message template</param>
        public static void Trace(string message, params object[] parameters)
        {
            if (Level < LogLevel.Trace)
            {
                return;
            }

            if (Instance is null)
            {
                Logger.Instance.Log(LogLevel.Trace, string.Format(message, parameters));
            }
            else
            {
                Instance.Trace(message, parameters);
            }
        }
    }
}
