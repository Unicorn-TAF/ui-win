namespace Unicorn.Taf.Api
{
    /// <summary>
    /// Interface of framework logger. Provides an ability to log message with specified severity.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs message with error level
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="parameters">parameters to substitute to message template</param>
        void Error(string message, params object[] parameters);

        /// <summary>
        /// Logs message with warning level
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="parameters">parameters to substitute to message template</param>
        void Warn(string message, params object[] parameters);

        /// <summary>
        /// Logs message with informational level.
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="parameters">parameters to substitute to message template</param>
        void Info(string message, params object[] parameters);

        /// <summary>
        /// Logs message with debug level
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="parameters">parameters to substitute to message template</param>
        void Debug(string message, params object[] parameters);

        /// <summary>
        /// Logs message with trace level
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="parameters">parameters to substitute to message template</param>
        void Trace(string message, params object[] parameters);
    }
}
