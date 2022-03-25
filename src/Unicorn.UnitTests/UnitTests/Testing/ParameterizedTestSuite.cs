using System;
using System.Reflection;
using NUnit.Framework;
using Unicorn.Taf.Core;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Suites;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Testing
{
    [TestFixture]
    public class ParameterizedTestSuite : NUnitTestRunner
    {
        private static TestsRunner runner;
        private static string executionOutput;

        private readonly UParameterizedSuite _suite = Activator.CreateInstance<UParameterizedSuite>();
        
        [OneTimeSetUp]
        public static void Setup()
        {
            Config.TestsExecutionOrder = TestsOrder.Declaration;
            Config.SetSuiteTags("parameterized");
            runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();
            executionOutput = UParameterizedSuite.Output;
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
            Test[] actualTests = (Test[])typeof(Taf.Core.Testing.TestSuite).GetField("_tests", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_suite);
            int testsCount = actualTests.Length;
            Assert.That(testsCount, Is.EqualTo(2));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that test suite determines correct count of After suite inside")]
        public void TestParameterizedSuiteCountOfAfterSuite() =>
            Assert.That(GetSuiteMethodListByName("_afterSuites").Length, Is.EqualTo(1));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that test suite determines correct count of before suite inside")]
        public void TestParameterizedSuiteCountOfBeforeSuite() =>
            Assert.That(GetSuiteMethodListByName("_beforeSuites").Length, Is.EqualTo(1));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that test suite determines correct count of After suite inside")]
        public void TestParameterizedSuiteCountOfAfterTest() =>
            Assert.That(GetSuiteMethodListByName("_afterTests").Length, Is.EqualTo(1));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that test suite determines correct count of before suite inside")]
        public void TestParameterizedSuiteCountOfBeforeTest() =>
            Assert.That(GetSuiteMethodListByName("_beforeTests").Length, Is.EqualTo(1));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check suite run")]
        public void TestParameterizedSuiteRunSuite()
        {
            string suiteOutputSet1 = "complex object with a = 2>BeforeSuite>BeforeTest>Test1>AfterTest>BeforeTest>Test2>AfterTest>AfterSuite";
            string suiteOutputSet2 = "complex object with b = 3>BeforeSuite>BeforeTest>Test1>AfterTest>BeforeTest>Test2>AfterTest>AfterSuite";
            
            Assert.That(executionOutput, Is.EqualTo(suiteOutputSet1 + suiteOutputSet2));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that test suite has different Id for run on each DataSet")]
        public void TestParameterizedSuiteExecutionsHasDifferentIds()
        {
            Assert.That(runner.Outcome.SuitesOutcomes.Count, Is.EqualTo(2));
            Assert.That(runner.Outcome.SuitesOutcomes[0].Id, Is.Not.EqualTo(runner.Outcome.SuitesOutcomes[1].Id));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that test suite has same Ids for different runs")]
        public void TestSuiteHasSameIdForDifferentRuns()
        {
            TestsRunner runner1 = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner1.RunTests();

            Assert.That(runner.Outcome.SuitesOutcomes[0].Id, Is.EqualTo(runner1.Outcome.SuitesOutcomes[0].Id));
            Assert.That(runner.Outcome.SuitesOutcomes[1].Id, Is.EqualTo(runner1.Outcome.SuitesOutcomes[1].Id));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that test suite test has different Id for run on each suite DataSet")]
        public void TestParameterizedSuiteTestExecutionsHasDifferentIds()
        {
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes[0].Id, 
                Is.Not.EqualTo(runner.Outcome.SuitesOutcomes[1].TestsOutcomes[0].Id));
        }

        private SuiteMethod[] GetSuiteMethodListByName(string name)
        {
            object field = typeof(Taf.Core.Testing.TestSuite)
                .GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(_suite);

            return field as SuiteMethod[];
        }
    }
}
