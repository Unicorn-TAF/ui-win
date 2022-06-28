using NUnit.Framework;
using System.IO;
using System.Reflection;
using Unicorn.Taf.Core.Logging;

#pragma warning disable S2187 // TestCases should contain tests
namespace Unicorn.UnitTests.Util
{
    [TestFixture]
    public class NUnitTestRunner
    {
        protected static string DllFolder { get; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        protected static string ConfigName { get; } = Path.Combine(DllFolder, "config.conf");

        [OneTimeSetUp]
        public static void ClassInit()
        {
            Logger.Instance = new TestContextLogger();
        }
    }
}
#pragma warning restore S2187 // TestCases should contain tests
