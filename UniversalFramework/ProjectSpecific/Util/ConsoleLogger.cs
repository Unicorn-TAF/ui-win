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
            if (parameters.Length > 0)
                TestContext.WriteLine("|\t\tDEBUG:" + string.Format(message, parameters));
            else
                TestContext.WriteLine("|\t\tDEBUG:" + message);
        }

        public void Error(string message, params object[] parameters)
        {
            if (parameters.Length > 0)
                TestContext.WriteLine("ERROR: " + string.Format(message, parameters));
            else
                TestContext.WriteLine("ERROR: " + message);
        }

        public void Info(string message, params object[] parameters)
        {
            if (parameters.Length > 0)
                TestContext.WriteLine(string.Format(message, parameters));
            else
                TestContext.WriteLine(message);
        }

        public void Init()
        {
        }
    }
}
