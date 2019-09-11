namespace Unicorn.Taf.Core.Logging
{
    /// <summary>
    /// Represents severity levels for framework logger.
    /// </summary>
    public enum LogLevel
    {
        Error,
        Warning,
        Info,
        Debug,
        Trace
    }

    /// <summary>
    /// Interface of framework logger. Provides an ability to log message with specified severity <see cref="LogLevel"/>
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log message with specified severity.
        /// </summary>
        /// <param name="level"><see cref="LogLevel"/> severity level</param>
        /// <param name="message">message to log</param>
        void Log(LogLevel level, string message);
    }
}
