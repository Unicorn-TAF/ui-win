using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using ProjectSpecific;
using Tests.TestData;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Adapter;

namespace Tests.UnitTests
{
    [TestFixture]
    public class TestSuiteTest : NUnitTestRunner
    {
        private Suite suite = Activator.CreateInstance<Suite>();

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of tests inside")]
        public void CountOfTestsTest()
        {
            Test[] actualTests = (Test[])typeof(TestSuite).GetField("tests", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(suite);
            int testsCount = actualTests.Length;
            Assert.That(testsCount, Is.EqualTo(3));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of After suite inside")]
        public void CountOfAfterSuiteTest()
        {
            Assert.That(GetSuiteMethodListByName("afterSuites").Length, Is.EqualTo(1));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of before suite inside")]
        public void CountOfBeforeSuiteTest()
        {
            Assert.That(GetSuiteMethodListByName("beforeSuites").Length, Is.EqualTo(1));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of After suite inside")]
        public void CountOfAfterTestTest()
        {
            Assert.That(GetSuiteMethodListByName("afterTests").Length, Is.EqualTo(1));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of before suite inside")]
        public void CountOfBeforeTestTest()
        {
            Assert.That(GetSuiteMethodListByName("beforeTests").Length, Is.EqualTo(1));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check suite run")]
        public void RunSuiteTest()
        {
            Suite.Output = string.Empty;
            string expectedOutput = "BeforeSuite>BeforeTest>Test1>AfterTest>BeforeTest>Test2>AfterTest>AfterSuite";
            Configuration.SetSuiteFeatures("sample");
            TestsRunner runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();
            Assert.That(Suite.Output, Is.EqualTo(expectedOutput));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Test For Suite Skipping")]
        public void SuiteSkipTest()
        {
            SuiteToBeSkipped.Output = string.Empty;
            Configuration.SetTestCategories("category");
            Configuration.SetSuiteFeatures("reporting");
            TestsRunner runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();
            Assert.That(SuiteToBeSkipped.Output, Is.EqualTo(string.Empty));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Test For Suite Skipping")]
        public void SuiteBugsTest()
        {
            SuiteForReporting repSuite = new SuiteForReporting();

            throw new NotImplementedException();
            ////repSuite.Run();
            string[] expectedBugs = new string[] { "234", "871236" };

            Assert.IsTrue(repSuite.Outcome.Bugs.Intersect(expectedBugs).Count() == 2);
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
