using NUnit.Framework;
using System;
using System.Reflection;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Suites;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Testing
{
    [TestFixture]
    public class TestSuite : NUnitTestRunner
    {
        private static USuite suite;

        [OneTimeSetUp]
        public static void SetUp()
        {
            Config.SetTestCategories();
            suite = Activator.CreateInstance<USuite>();
        }

        public static void TearDown() =>
            suite = null;

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that test suite determines correct count of tests inside")]
        public void TestSuitesCountOfTests()
        {
            Test[] actualTests = (Test[])typeof(Taf.Core.Testing.TestSuite).GetField("_tests", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(suite);
            int testsCount = actualTests.Length;
            Assert.That(testsCount, Is.EqualTo(2));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that test suite determines correct count of After suite inside")]
        public void TestSuitesCountOfAfterSuite() =>
            Assert.That(GetSuiteMethodListByName("_afterSuites").Length, Is.EqualTo(1));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that test suite determines correct count of before suite inside")]
        public void TestSuitesCountOfBeforeSuite() =>
            Assert.That(GetSuiteMethodListByName("_beforeSuites").Length, Is.EqualTo(1));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that test suite determines correct count of After suite inside")]
        public void TestSuitesCountOfAfterTest() =>
            Assert.That(GetSuiteMethodListByName("_afterTests").Length, Is.EqualTo(1));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that test suite determines correct count of before suite inside")]
        public void TestSuitesCountOfBeforeTest() =>
            Assert.That(GetSuiteMethodListByName("_beforeTests").Length, Is.EqualTo(1));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check suite run")]
        public void TestSuitesRunSuite()
        {
            USuite.Output = string.Empty;
            string expectedOutput = "BeforeSuite>BeforeTest>Test1>AfterTest>BeforeTest>Test2>AfterTest>AfterSuite";
            Config.SetSuiteTags("sample");
            TestsRunner runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
            runner.RunTests();
            Assert.That(USuite.Output, Is.EqualTo(expectedOutput));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test For Suite Skipping")]
        public void TestSuitesSuiteSkip()
        {
            USuiteToBeSkipped.Output = string.Empty;
            Config.SetTestCategories("category");
            Config.SetSuiteTags("reporting");
            TestsRunner runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
            runner.RunTests();
            Assert.That(USuiteToBeSkipped.Output, Is.EqualTo(string.Empty));
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
