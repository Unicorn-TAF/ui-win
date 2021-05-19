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
    public class ParameterizedTests : NUnitTestRunner
    {
        private static TestsRunner runner;
        private readonly USuiteWithParameterizedTests suite = Activator.CreateInstance<USuiteWithParameterizedTests>();
        
        [OneTimeSetUp]
        public static void Setup()
        {
            Config.SetSuiteTags("parameterizedTests");
            Config.TestsExecutionOrder = TestsOrder.Declaration;
            runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
        }

        [OneTimeTearDown]
        public static void Cleanup() =>
            runner = null;

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that test suite determines correct count of tests inside")]
        public void TestParameterizedSuiteCountOfTests()
        {
            Test[] actualTests = (Test[])typeof(Taf.Core.Testing.TestSuite).GetField("_tests", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(suite);
            int testsCount = actualTests.Length;
            Assert.That(testsCount, Is.EqualTo(3));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check suite run")]
        public void TestParameterizedSuiteRunSuite()
        {
            USuiteWithParameterizedTests.Output = string.Empty;
            string suiteOutputSet1 = "BeforeSuite>BeforeTest>complex object with b = 3Test1>AfterTest>BeforeTest>complex object with a = 2Test1>AfterTest>BeforeTest>Test2>AfterTest>AfterSuite";
            runner.RunTests();
            Assert.That(USuiteWithParameterizedTests.Output, Is.EqualTo(suiteOutputSet1));
        }
    }
}
