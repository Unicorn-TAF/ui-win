using NUnit.Framework;
using System.Reflection;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Steps;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Testing
{
    [TestFixture]
    public class StepsFeature : NUnitTestRunner
    {
        private string Output = string.Empty; 
        
        [SetUp]
        public void Setup() =>
            StepsEvents.OnStepStart += ReportInfo;

        [TearDown]
        public void Cleanup() =>
            StepsEvents.OnStepStart -= ReportInfo;

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check step events")]
        public void TestStepEvents()
        {
            Config.SetSuiteTags("Steps");
            new TestsRunner(Assembly.GetExecutingAssembly().Location, false).RunTests();
            Assert.That(Output, Is.EqualTo("Say 'Test1'Say 'AfterTest'Say 'Test2'Say 'AfterTest'Say 'AfterSuite'"));
        }

        private void ReportInfo(MethodBase method, object[] arguments) =>
            Output += StepsUtilities.GetStepInfo(method, arguments);
    }
}
