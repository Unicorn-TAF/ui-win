using NUnit.Framework;
using ProjectSpecific;
using System;
using System.Collections.Generic;
using System.Reflection;
using Tests.TestData;
using Unicorn.Core.Testing.Tests;

namespace Tests.UnitTests
{
    [TestFixture]
    public class TestParameterizedTestSuiteTest : NUnitTestRunner
    {
        private ParameterizedSuite suite = Activator.CreateInstance<ParameterizedSuite>();

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of tests inside")]
        public void ParameterizedSuiteCountOfTestsTest()
        {
            List<Test>[] actualTests = (List<Test>[])typeof(TestSuite).GetField("ListTestsAll", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(suite);
            int testsCount = actualTests[0].Count * actualTests.Length;
            Assert.That(testsCount, Is.EqualTo(6));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of After suite inside")]
        public void ParameterizedSuiteCountOfAfterSuiteTest()
        {
            Assert.That(GetSuiteMethodListByName("ListAfterSuite").Length, Is.EqualTo(1));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of before suite inside")]
        public void ParameterizedSuiteCountOfBeforeSuiteTest()
        {
            Assert.That(GetSuiteMethodListByName("ListBeforeSuite").Length, Is.EqualTo(1));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of After suite inside")]
        public void ParameterizedSuiteCountOfAfterTestTest()
        {
            Assert.That(GetListByName("ListAfterTest").Length, Is.EqualTo(1));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of before suite inside")]
        public void ParameterizedSuiteCountOfBeforeTestTest()
        {
            Assert.That(GetListByName("ListBeforeTest").Length, Is.EqualTo(1));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check suite run")]
        public void ParameterizedSuiteRunSuiteTest()
        {
            string expectedOutput = "BeforeSuite>BeforeTest>Test1>AfterTest>BeforeTest>Test2>AfterTest>AfterSuite";
            suite.Run();
            Assert.That(suite.GetOutput(), Is.EqualTo(expectedOutput + expectedOutput));
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
