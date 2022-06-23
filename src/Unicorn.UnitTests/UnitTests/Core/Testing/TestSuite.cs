using NUnit.Framework;
using System;
using System.Reflection;
using Unicorn.Taf.Core;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Suites;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Core.Testing
{
    [TestFixture]
    public class TestSuite : NUnitTestRunner
    {
        private static USuite suite;

        [OneTimeSetUp]
        public static void SetUp()
        {
            Config.SetTestCategories();
            Config.TestsExecutionOrder = TestsOrder.Declaration;
            suite = Activator.CreateInstance<USuite>();
        }

        [OneTimeTearDown]
        public static void ResetConfig()
        {
            Config.Reset();
            suite = null;
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that test suite determines correct count of tests inside")]
        public void TestSuitesCountOfTests()
        {
            Test[] actualTests = (Test[])typeof(Taf.Core.Testing.TestSuite)
                .GetField("_tests", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(suite);

            int testsCount = actualTests.Length;
            Assert.That(testsCount, Is.EqualTo(3));
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
        [Test(Description = "Check that test suite matadata could be retreived")]
        public void TestSuiteMetadata()
        {
            Assert.That(suite.Metadata.Count, Is.EqualTo(2));
            Assert.That(suite.Metadata["key1"], Is.EqualTo("value1"));
            Assert.That(suite.Metadata["key2"], Is.EqualTo("value2"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check suite run")]
        public void TestSuitesRunSuite()
        {
            USuite.Output = string.Empty;
            string expectedOutput = 
                "BeforeSuite>BeforeTest>Test1>AfterTest>BeforeTest>Test11>AfterTest>BeforeTest>Test2>AfterTest>AfterSuite";

            Config.SetSuiteTags(Tag.Sample);
            TestsRunner runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();
            Assert.That(USuite.Output, Is.EqualTo(expectedOutput));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test For Suite Skipping")]
        public void TestSuitesSuiteSkip()
        {
            USuiteToBeSkipped.Output = string.Empty;
            Config.SetTestCategories("category");
            Config.SetSuiteTags(Tag.Skipping);
            TestsRunner runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();
            Assert.That(USuiteToBeSkipped.Output, Is.EqualTo(string.Empty));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test For Suite Disabling")]
        public void TestSuitesDisabledSuite()
        {
            USuiteDisabled.Output = string.Empty;
            Config.SetSuiteTags(Tag.Disabled);
            TestsRunner runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();
            Assert.That(USuiteDisabled.Output, Is.EqualTo(string.Empty));
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
