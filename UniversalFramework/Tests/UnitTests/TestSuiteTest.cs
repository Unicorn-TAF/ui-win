using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Tests.TestData;
using Unicorn.Core.Testing.Tests;

namespace Tests.UnitTests
{
    [TestFixture]
    class TestSuiteTest
    {
        Suite suite = Activator.CreateInstance<Suite>();

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of tests inside")]
        public void CountOfTestsTest()
        {
            List<Test> actualTests = (List<Test>)typeof(TestSuite).GetField("ListTests", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(suite);
            Assert.That(actualTests.Count, Is.EqualTo(3));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of After suite inside")]
        public void CountOfAfterSuiteTest()
        {
            Assert.That(GetListByName("ListAfterSuite").Length, Is.EqualTo(1));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of before suite inside")]
        public void CountOfBeforeSuiteTest()
        {
            Assert.That(GetListByName("ListBeforeSuite").Length, Is.EqualTo(1));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of After suite inside")]
        public void CountOfAfterTestTest()
        {
            Assert.That(GetListByName("ListAfterTest").Length, Is.EqualTo(1));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of before suite inside")]
        public void CountOfBeforeTestTest()
        {
            Assert.That(GetListByName("ListBeforeTest").Length, Is.EqualTo(1));
        }


        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check suite run")]
        public void RunSuiteTest()
        {
            string expectedOutput = "BeforeSuite>BeforeTest>Test1>AfterTest>BeforeTest>Test2>AfterTest>AfterSuite";
            suite.Run();
            Assert.That(suite.Output, Is.EqualTo(expectedOutput));
        }


        private MethodInfo[] GetListByName(string name)
        {
            object field = typeof(TestSuite)
                .GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(suite);

            return field as MethodInfo[];
        }



        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Test For Suite Skipping")]
        public void SuiteSkipTest()
        {
            TestSuite.SetRunCategories("category");
            List<Type> suitesList = new List<Type>();
            suitesList.Add(typeof(SuiteToBeSkipped));

            foreach(Type type in suitesList)
            {
                var suite = Activator.CreateInstance(type);
                ((TestSuite)suite).Run();

                Assert.That(((SuiteToBeSkipped)suite).Output, Is.EqualTo(""));
            }
        }
    }
}
