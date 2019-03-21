namespace Unicorn.Taf.Core.Logging
{
    public enum LogLevel
    {
        Error,
        Warning,
        Info,
        Debug,
        Trace
    }

    public interface ILogger
    {
        void Log(LogLevel level, string message);
    }
}
