using NUnit.Framework;
using System.Reflection;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Testing
{
    [TestFixture]
    public class TestsExecutionOrder : NUnitTestRunner
    {
        [OneTimeSetUp]
        public static void Setup() =>
            Config.SetSuiteTags("tests-order");

        [OneTimeTearDown]
        public static void Cleanup() =>
            Config.Reset();

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Declaration order of tests execution")]
        public void TestDeclarationOrderOfTestsExecution()
        {
            Config.TestsExecutionOrder = TestsOrder.Declaration;
            var runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
            runner.RunTests();
            var outcome = runner.Outcome.SuitesOutcomes[0];

            Assert.That(outcome.TestsOutcomes[0].Title, Is.EqualTo("Test2"));
            Assert.That(outcome.TestsOutcomes[1].Title, Is.EqualTo("Test4"));
            Assert.That(outcome.TestsOutcomes[2].Title, Is.EqualTo("Test3"));
            Assert.That(outcome.TestsOutcomes[3].Title, Is.EqualTo("Test6"));
            Assert.That(outcome.TestsOutcomes[4].Title, Is.EqualTo("Test5"));
            Assert.That(outcome.TestsOutcomes[5].Title, Is.EqualTo("Test1"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Declaration order of tests execution")]
        public void TestRandomOrderOfTestsExecution()
        {
            Config.TestsExecutionOrder = TestsOrder.Random;
            var runner1 = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
            runner1.RunTests();

            var outcome1 = runner1.Outcome.SuitesOutcomes[0];

            System.Threading.Thread.Sleep(250);

            var runner2 = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
            runner2.RunTests();

            var outcome2 = runner2.Outcome.SuitesOutcomes[0];
            var orderIsTheSame = outcome2.TestsOutcomes[0].Title.Equals(outcome1.TestsOutcomes[0].Title);
            orderIsTheSame &= outcome2.TestsOutcomes[1].Title.Equals(outcome1.TestsOutcomes[1].Title);
            orderIsTheSame &= outcome2.TestsOutcomes[2].Title.Equals(outcome1.TestsOutcomes[2].Title);
            orderIsTheSame &= outcome2.TestsOutcomes[3].Title.Equals(outcome1.TestsOutcomes[3].Title);
            orderIsTheSame &= outcome2.TestsOutcomes[4].Title.Equals(outcome1.TestsOutcomes[4].Title);
            orderIsTheSame &= outcome2.TestsOutcomes[5].Title.Equals(outcome1.TestsOutcomes[5].Title);
            
            Assert.False(orderIsTheSame);

        }
    }
}
