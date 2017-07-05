namespace Unicorn.Core.Logging
{
    public interface ILogger
    {

        void Init();

        void Info(string message, params string[] parameters);


        void Debug(string message, params string[] parameters);
    }
}
