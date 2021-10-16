using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Suites;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Testing
{
    [TestFixture]
    public class ParameterizedTests : NUnitTestRunner
    {
        private static TestsRunner runner;
        private static string executionOutput;

        [OneTimeSetUp]
        public static void Setup()
        {
            Config.SetSuiteTags("parameterizedTests");
            Config.TestsExecutionOrder = TestsOrder.Declaration;

            runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
            runner.RunTests();
            executionOutput = USuiteWithParameterizedTests.Output;
        }

        [OneTimeTearDown]
        public static void Cleanup()
        {
            runner = null;
            executionOutput = null;
            Config.Reset();
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that test suite determines correct count of tests inside")]
        public void TestParameterizedSuiteCountOfTests()
        {
            var suite = Activator.CreateInstance<USuiteWithParameterizedTests>();

            Test[] actualTests = typeof(Taf.Core.Testing.TestSuite)
                .GetField("_tests", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(suite)
                as Test[];

            Assert.That(actualTests.Length, Is.EqualTo(3));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check suite run")]
        public void TestParameterizedSuiteRunSuite()
        {
            string suiteOutputSet1 = "BeforeSuite>BeforeTest>complex object with b = 3Test1>AfterTest>BeforeTest>complex object with a = 2Test1>AfterTest>BeforeTest>Test2>AfterTest>AfterSuite";

            Assert.That(executionOutput, Is.EqualTo(suiteOutputSet1));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that test suite has different Id for run on each DataSet")]
        public void TestParameterizedTestExecutionsHasDifferentIds()
        {
            var testOutcomes = runner.Outcome.SuitesOutcomes[0].TestsOutcomes
                .Where(to => to.Title.StartsWith("Test 1"));

            Assert.That(testOutcomes.Count(), Is.EqualTo(2));

            Assert.That(testOutcomes.ElementAt(0).Id, Is.Not.EqualTo(testOutcomes.ElementAt(1).Id));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that test suite has same Ids for different runs")]
        public void TestSuiteHasSameIdForDifferentRuns()
        {
            TestsRunner runner1 = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
            runner1.RunTests();

            var testOutcomes = runner.Outcome.SuitesOutcomes[0].TestsOutcomes
                .Where(to => to.Title.StartsWith("Test 1"));

            var testOutcomes1 = runner1.Outcome.SuitesOutcomes[0].TestsOutcomes
                .Where(to => to.Title.StartsWith("Test 1"));

            Assert.That(testOutcomes.ElementAt(0).Id, Is.EqualTo(testOutcomes1.ElementAt(0).Id));
            Assert.That(testOutcomes.ElementAt(1).Id, Is.EqualTo(testOutcomes1.ElementAt(1).Id));
        }
    }
}
