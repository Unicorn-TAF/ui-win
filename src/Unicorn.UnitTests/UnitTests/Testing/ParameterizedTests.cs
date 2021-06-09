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
        [OneTimeSetUp]
        public static void Setup()
        {
            Config.SetSuiteTags("parameterizedTests");
            Config.TestsExecutionOrder = TestsOrder.Declaration;
        }

        [OneTimeTearDown]
        public static void Cleanup() =>
            Config.Reset();

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
            USuiteWithParameterizedTests.Output = string.Empty;
            string suiteOutputSet1 = "BeforeSuite>BeforeTest>complex object with b = 3Test1>AfterTest>BeforeTest>complex object with a = 2Test1>AfterTest>BeforeTest>Test2>AfterTest>AfterSuite";

            var runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
            runner.RunTests();
            Assert.That(USuiteWithParameterizedTests.Output, Is.EqualTo(suiteOutputSet1));
        }
    }
}
