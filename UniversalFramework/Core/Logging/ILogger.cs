namespace Unicorn.Core.Logging
{
    public enum LogLevel
    {
        Info,
        Debug,
        Warning,
        Error
    }

    public interface ILogger
    {
        void Log(LogLevel level, string message);
    }
}
