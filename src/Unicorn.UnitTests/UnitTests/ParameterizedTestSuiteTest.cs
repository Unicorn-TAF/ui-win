using System;
using System.Reflection;
using NUnit.Framework;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Adapter;
using Unicorn.UnitTests.Suites;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Tests
{
    [TestFixture]
    public class TestParameterizedTestSuiteTest : NUnitTestRunner
    {
        private static TestsRunner runner;

        private readonly ParameterizedSuite suite = Activator.CreateInstance<ParameterizedSuite>();
        
        [OneTimeSetUp]
        public static void Setup()
        {
            Configuration.SetSuiteFeatures("parameterized");
            runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of tests inside")]
        public void TestParameterizedSuiteCountOfTests()
        {
            Test[] actualTests = (Test[])typeof(TestSuite).GetField("tests", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(suite);
            int testsCount = actualTests.Length;
            Assert.That(testsCount, Is.EqualTo(3));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of After suite inside")]
        public void TestParameterizedSuiteCountOfAfterSuite()
        {
            Assert.That(GetSuiteMethodListByName("afterSuites").Length, Is.EqualTo(1));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of before suite inside")]
        public void TestParameterizedSuiteCountOfBeforeSuite()
        {
            Assert.That(GetSuiteMethodListByName("beforeSuites").Length, Is.EqualTo(1));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of After suite inside")]
        public void TestParameterizedSuiteCountOfAfterTest()
        {
            Assert.That(GetSuiteMethodListByName("afterTests").Length, Is.EqualTo(1));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of before suite inside")]
        public void TestParameterizedSuiteCountOfBeforeTest()
        {
            Assert.That(GetSuiteMethodListByName("beforeTests").Length, Is.EqualTo(1));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check suite run")]
        public void TestParameterizedSuiteRunSuite()
        {
            ParameterizedSuite.Output = string.Empty;
            string suiteOutputSet1 = "complex object with a = 2>BeforeSuite>BeforeTest>Test1>AfterTest>BeforeTest>Test2>AfterTest>AfterSuite";
            string suiteOutputSet2 = "complex object with b = 3>BeforeSuite>BeforeTest>Test1>AfterTest>BeforeTest>Test2>AfterTest>AfterSuite";
            runner.RunTests();
            Assert.That(ParameterizedSuite.Output, Is.EqualTo(suiteOutputSet1 + suiteOutputSet2));
        }

        private SuiteMethod[] GetSuiteMethodListByName(string name)
        {
            object field = typeof(TestSuite)
                .GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(suite);

            return field as SuiteMethod[];
        }
    }
}
