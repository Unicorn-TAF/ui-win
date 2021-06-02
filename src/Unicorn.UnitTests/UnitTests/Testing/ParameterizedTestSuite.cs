using System;
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
    public class ParameterizedTestSuite : NUnitTestRunner
    {
        private static TestsRunner runner;

        private readonly UParameterizedSuite suite = Activator.CreateInstance<UParameterizedSuite>();
        
        [OneTimeSetUp]
        public static void Setup()
        {
            Config.TestsExecutionOrder = TestsOrder.Declaration;
            Config.SetSuiteTags("parameterized");
            runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
        }

        [OneTimeTearDown]
        public static void Cleanup()
        {
            runner = null;
            Config.Reset();
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that test suite determines correct count of tests inside")]
        public void TestParameterizedSuiteCountOfTests()
        {
            Test[] actualTests = (Test[])typeof(Taf.Core.Testing.TestSuite).GetField("_tests", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(suite);
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
            UParameterizedSuite.Output = string.Empty;
            string suiteOutputSet1 = "complex object with a = 2>BeforeSuite>BeforeTest>Test1>AfterTest>BeforeTest>Test2>AfterTest>AfterSuite";
            string suiteOutputSet2 = "complex object with b = 3>BeforeSuite>BeforeTest>Test1>AfterTest>BeforeTest>Test2>AfterTest>AfterSuite";
            runner.RunTests();
            Assert.That(UParameterizedSuite.Output, Is.EqualTo(suiteOutputSet1 + suiteOutputSet2));
        }

        private SuiteMethod[] GetSuiteMethodListByName(string name)
        {
            object field = typeof(Taf.Core.Testing.TestSuite)
                .GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(suite);

            return field as SuiteMethod[];
        }
    }
}
