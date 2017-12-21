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
            List<Test>[] actualTests = (List<Test>[])typeof(TestSuite).GetField("listTestsAll", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(suite);
            int testsCount = actualTests[0].Count * actualTests.Length;
            Assert.That(testsCount, Is.EqualTo(3));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of After suite inside")]
        public void CountOfAfterSuiteTest()
        {
            Assert.That(GetSuiteMethodListByName("listAfterSuite").Length, Is.EqualTo(1));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of before suite inside")]
        public void CountOfBeforeSuiteTest()
        {
            Assert.That(GetSuiteMethodListByName("listBeforeSuite").Length, Is.EqualTo(1));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of After suite inside")]
        public void CountOfAfterTestTest()
        {
            Assert.That(GetListByName("listAfterTest").Length, Is.EqualTo(1));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of before suite inside")]
        public void CountOfBeforeTestTest()
        {
            Assert.That(GetListByName("listBeforeTest").Length, Is.EqualTo(1));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check suite run")]
        public void RunSuiteTest()
        {
            string expectedOutput = "BeforeSuite>BeforeTest>Test1>AfterTest>BeforeTest>Test2>AfterSuite";
            suite.Run();
            Assert.That(suite.GetOutput(), Is.EqualTo(expectedOutput));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Test For Suite Skipping")]
        public void SuiteSkipTest()
        {
            Configuration.SetTestCategories("category");
            List<Type> suitesList = new List<Type>();
            suitesList.Add(typeof(SuiteToBeSkipped));

            foreach (Type type in suitesList)
            {
                var suite = Activator.CreateInstance(type);
                ((TestSuite)suite).Run();
                Configuration.SetTestCategories("category");
                Assert.That(((SuiteToBeSkipped)suite).GetOutput(), Is.EqualTo(string.Empty));
            }
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Test For Suite Skipping")]
        public void SuiteBugsTest()
        {
            SuiteForReporting repSuite = new SuiteForReporting();

            repSuite.Run();
            string[] expectedBugs = new string[] { "234", "871236" };

            Assert.IsTrue(repSuite.Outcome.Bugs.Intersect(expectedBugs).Count() == 2);
        }

        private MethodInfo[] GetListByName(string name)
        {
            object field = typeof(TestSuite)
                .GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(suite);

            return field as MethodInfo[];
        }

        private TestSuiteMethod[] GetSuiteMethodListByName(string name)
        {
            object field = typeof(TestSuite)
                .GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(suite);

            return field as TestSuiteMethod[];
        }
    }
}
