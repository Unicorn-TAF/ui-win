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
            Assert.That(GetListByName("ListTests").Count, Is.EqualTo(3));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of After suite inside")]
        public void CountOfAfterSuiteTest()
        {
            Assert.That(GetListByName("ListAfterSuite").Count, Is.EqualTo(1));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of before suite inside")]
        public void CountOfBeforeSuiteTest()
        {
            Assert.That(GetListByName("ListBeforeSuite").Count, Is.EqualTo(1));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of After suite inside")]
        public void CountOfAfterTestTest()
        {
            Assert.That(GetListByName("ListAfterTest").Count, Is.EqualTo(1));
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that test suite determines correct count of before suite inside")]
        public void CountOfBeforeTestTest()
        {
            Assert.That(GetListByName("ListBeforeTest").Count, Is.EqualTo(1));
        }


        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check suite run")]
        public void RunSuiteTest()
        {
            string expectedOutput = "BeforeSuite>BeforeTest>Test1>AfterTest>BeforeTest>Test2>AfterTest>AfterSuite";
            suite.Run();
            Assert.That(suite.Output, Is.EqualTo(expectedOutput));
        }

        private List<MethodInfo> GetListByName(string name)
        {
            return (List <MethodInfo>)typeof(TestSuite).GetField(name, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(suite);
        }

    }
}
