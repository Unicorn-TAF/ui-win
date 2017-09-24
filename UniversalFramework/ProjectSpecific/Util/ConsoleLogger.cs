using Unicorn.Core.Logging;
using NUnit.Framework;
using System;
using Unicorn.Core.Testing.Tests;

namespace ProjectSpecific.Util
{
    public class ConsoleLogger : ILogger
    {
        public void Debug(string message, params object[] parameters)
        {
            TestContext.WriteLine("|\t\tDEBUG:" + string.Format(message, parameters));
        }

        public void Error(string message, params object[] parameters)
        {
            TestContext.WriteLine("ERROR: " + string.Format(message, parameters));
        }

        public void Info(string message, params object[] parameters)
        {
            TestContext.WriteLine(string.Format(message, parameters));
        }

        public void Init()
        {
        }
    }
}
