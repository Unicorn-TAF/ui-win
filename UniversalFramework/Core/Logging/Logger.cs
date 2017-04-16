using NUnit.Framework;

namespace Core.Logging
{
    public class Logger
    {
        public static void Init() { }

        public static void Info(string message, params string[] parameters)
        {
            TestContext.WriteLine(string.Format(message, parameters));
        }

        public static void Debug(string message, params string[] parameters)
        {
            TestContext.WriteLine("|\t\t" + string.Format(message, parameters));
        }

    }
}
