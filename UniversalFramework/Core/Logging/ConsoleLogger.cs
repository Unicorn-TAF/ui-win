using System;

namespace Unicorn.Core.Logging
{
    class ConsoleLogger : ILogger
    {
        public void Debug(string message, params object[] parameters)
        {
            System.Diagnostics.Debug.WriteLine("|\t\tDEBUG: " + string.Format(message, parameters));
        }

        public void Error(string message, params object[] parameters)
        {
            System.Diagnostics.Debug.WriteLine("ERROR: " + string.Format(message, parameters));
        }

        public void Info(string message, params object[] parameters)
        {
            System.Diagnostics.Debug.WriteLine(string.Format(message, parameters));
        }

        public void Init()
        {
            throw new NotImplementedException();
        }
    }
}
