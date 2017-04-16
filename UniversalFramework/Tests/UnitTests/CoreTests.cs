using Core.Logging;
using NUnit.Framework;

namespace Tests.UnitTests
{
    class CoreTests
    {
        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Test For check logging")]
        public void DebugTest()
        {
            Logger.Info("simple text");
            Logger.Debug("simple text debug");
            Logger.Info("text with parameters: first parameter '{0}', second - '{1}'", "qwerty", 23.ToString());
            Logger.Debug("debug of text with parameters: first parameter '{0}', second - '{1}'", "qwerty", 23.ToString());
        }


    }
}
