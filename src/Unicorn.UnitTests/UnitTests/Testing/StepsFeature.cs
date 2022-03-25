using NUnit.Framework;
using System.Reflection;
using Unicorn.Taf.Core;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Steps;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Testing
{
    [TestFixture]
    public class StepsFeature : NUnitTestRunner
    {
        private static string Output = string.Empty;

        [OneTimeSetUp]
        public static void SetConfig()
        {
            Config.TestsExecutionOrder = TestsOrder.Declaration;
            Config.SetSuiteTags("Steps");
            StepsEvents.OnStepStart += ReportInfo;
        }

        [OneTimeTearDown]
        public static void ResetConfig()
        {
            Config.Reset();
            StepsEvents.OnStepStart -= ReportInfo;
            Output = null;
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check step events")]
        public void TestStepEvents()
        {
            new TestsRunner(Assembly.GetExecutingAssembly(), false).RunTests();
            Assert.That(
                Output, 
                Is.EqualTo("Test1AfterTestAssert that 'Test2' Is equal to Test2AfterTestAfterSuite"));
        }

        private static void ReportInfo(MethodBase method, object[] arguments) =>
            Output += StepsUtilities.GetStepInfo(method, arguments);
    }
}
