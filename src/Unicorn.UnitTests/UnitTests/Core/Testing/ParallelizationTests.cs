using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
using Unicorn.Taf.Core;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Suites;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Core.Testing
{
    [TestFixture]
    public class ParallelizationTests : NUnitTestRunner
    {
        private static TestsRunner runner;
        private static List<string> executedTestsOrder = new List<string>();

        [OneTimeSetUp]
        public static void SetConfig()
        {
            Config.TestsExecutionOrder = TestsOrder.Alphabetical;
            Config.SetSuiteTags(Tag.SuiteParallelizaton);
            Config.ParallelBy = Parallelization.Suite;
            Config.Threads = 2;
            ParallelSuitesHelper.Reset();
            Test.OnTestFinish += OnTestFinish;

            runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
        }

        public static void OnTestFinish(Test test)
        {
            executedTestsOrder.Add(test.Outcome.Title);
        }

        [OneTimeTearDown]
        public static void ResetConfig()
        {
            Config.Reset();
            Test.OnTestFinish -= OnTestFinish;
            executedTestsOrder = null;
            runner = null;
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Parallelization by suites")]
        public void TestParallelizationBySuites()
        {
            runner.RunTests();

            Assert.IsTrue(executedTestsOrder.Count == 6);
            Assert.That(executedTestsOrder[0], Is.EqualTo(nameof(UParallelizationSuite1.ParallelTest11)));
            Assert.That(executedTestsOrder[1], Is.EqualTo(nameof(UParallelizationSuite2.ParallelTest21)));
            Assert.That(executedTestsOrder[2], Is.EqualTo(nameof(UParallelizationSuite1.ParallelTest12)));
            Assert.That(executedTestsOrder[3], Is.EqualTo(nameof(UParallelizationSuite2.ParallelTest22)));
            Assert.That(executedTestsOrder[4], Is.EqualTo(nameof(UParallelizationSuite2.ParallelTest23)));
            Assert.That(executedTestsOrder[5], Is.EqualTo(nameof(UParallelizationSuite1.ParallelTest13)));
        }
    }
}
