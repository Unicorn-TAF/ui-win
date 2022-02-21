using System.IO;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Win;

#pragma warning disable S2187 // TestCases should contain tests
namespace Unicorn.UnitTests.Util
{
    [TestFixture]
    public class NUnitTestRunner
    {
        public static SimpleReporter Reporter { get; set; }

        public static WinScreenshotTaker Screenshot { get; set; }

        [OneTimeSetUp]
        public static void ClassInit()
        {
            Logger.Instance = new TestContextLogger();
            Screenshot = new WinScreenshotTaker();
            Reporter = new SimpleReporter();
        }

        [OneTimeTearDown]
        public static void ClassCleanup()
        {
            Screenshot = null;
            Reporter = null;
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
