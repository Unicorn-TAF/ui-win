namespace Unicorn.Core.Logging
{
    public interface ILogger
    {

        void Init();

        void Info(string message, params object[] parameters);


        void Debug(string message, params object[] parameters);


        void Error(string message, params object[] parameters);
    }
}
