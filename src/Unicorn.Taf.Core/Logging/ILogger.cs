namespace Unicorn.Taf.Core.Logging
{
    /// <summary>
    /// Represents severity levels for framework logger.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Used to log errors which are not handled.
        /// </summary>
        Error,

        /// <summary>
        /// Used to log errors which are handled.
        /// </summary>
        Warning,

        /// <summary>
        /// Used to log general information.
        /// </summary>
        Info,

        /// <summary>
        /// Used to log information for debugging.
        /// </summary>
        Debug,

        /// <summary>
        /// Used to log low level information.
        /// </summary>
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
