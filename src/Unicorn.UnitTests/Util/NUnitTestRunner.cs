using System.IO;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using Unicorn.Core.Logging;
using Unicorn.Core.Reporting;

#pragma warning disable S2187 // TestCases should contain tests
namespace Unicorn.UnitTests.Util
{
    [TestFixture]
    public class NUnitTestRunner
    {
        [OneTimeSetUp]
        public static void ClassInit()
        {
            Logger.Instance = new TestContextLogger();
            Reporter.Instance = new SimpleReporter();
            Reporter.Instance.Init();
        }

        protected string GetTestContextOut()
        {
            TextWriter tout = TestContext.Out;
            TextWriter tout1 = (TextWriter)tout.GetType().GetField("_out", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(tout);
            StringBuilder sb = (StringBuilder)tout1.GetType().GetField("_sb", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(tout1);
            return sb.ToString();
        }
    }
}
#pragma warning restore S2187 // TestCases should contain tests
