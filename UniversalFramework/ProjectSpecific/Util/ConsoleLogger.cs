using Unicorn.Core.Logging;
using NUnit.Framework;

namespace ProjectSpecific.Util
{
    class ConsoleLogger : ILogger
    {
        public void Debug(string message, params string[] parameters)
        {
            TestContext.WriteLine(string.Format(message, parameters));
        }

        public void Info(string message, params string[] parameters)
        {
            TestContext.WriteLine("|\t\t" + string.Format(message, parameters));
        }

        public void Init()
        {
            
        }
    }
}
