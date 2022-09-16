using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
using Unicorn.Taf.Core;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Suites;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Core.Testing
{
    [TestFixture(TafEventsSuiteMode.RunAll)]
    [TestFixture(TafEventsSuiteMode.FailBeforeSuite)]
    [TestFixture(TafEventsSuiteMode.FailBeforeTest)]
    public class TafEventsTests : NUnitTestRunner
    {
        private readonly List<string> _expectedOutput;

        public TafEventsTests(TafEventsSuiteMode runMode)
        {
            USuiteForTafEvents.RunMode = runMode;
            USuiteForTafEvents.Output.Clear();

            switch(runMode)
            {
                case TafEventsSuiteMode.RunAll:
                    _expectedOutput = GetRunAllExpecteds();
                    break;
                case TafEventsSuiteMode.FailBeforeSuite:
                    _expectedOutput = GetFailBeforeSuiteExpecteds();
                    break;
                case TafEventsSuiteMode.FailBeforeTest:
                    _expectedOutput = GetFailBeforeTestExpecteds();
                    break;
            }
        }

        [OneTimeSetUp]
        public static void SetConfig()
        {
            Config.TestsExecutionOrder = TestsOrder.Alphabetical;
            Config.SetSuiteTags(Tag.TafEvents);

            TestSuite.OnSuiteStart += SuiteStartEvent;
            TestSuite.OnSuiteFinish += SuiteFinishEvent;
            TestSuite.OnSuiteSkip += SuiteSkipEvent;

            SuiteMethod.OnSuiteMethodStart += SuiteMethodStartEvent;
            SuiteMethod.OnSuiteMethodFinish += SuiteMethodFinishEvent;
            SuiteMethod.OnSuiteMethodPass += SuiteMethodPassEvent;
            SuiteMethod.OnSuiteMethodFail += SuiteMethodFailEvent;

            Test.OnTestStart += TestStartEvent;
            Test.OnTestFinish += TestFinishEvent;
            Test.OnTestPass += TestPassEvent;
            Test.OnTestFail += TestFailEvent;
            Test.OnTestSkip += TestSkipEvent;
        }

        [OneTimeTearDown]
        public static void ResetConfig()
        {
            Config.Reset();
            USuiteForTafEvents.Output.Clear();

            TestSuite.OnSuiteStart -= SuiteStartEvent;
            TestSuite.OnSuiteFinish -= SuiteFinishEvent;
            TestSuite.OnSuiteSkip -= SuiteSkipEvent;

            SuiteMethod.OnSuiteMethodStart -= SuiteMethodStartEvent;
            SuiteMethod.OnSuiteMethodFinish -= SuiteMethodFinishEvent;
            SuiteMethod.OnSuiteMethodPass -= SuiteMethodPassEvent;
            SuiteMethod.OnSuiteMethodFail -= SuiteMethodFailEvent;

            Test.OnTestStart -= TestStartEvent;
            Test.OnTestFinish -= TestFinishEvent;
            Test.OnTestPass -= TestPassEvent;
            Test.OnTestFail -= TestFailEvent;
            Test.OnTestSkip -= TestSkipEvent;
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check dependent tests don't run on TestsDependency.DoNotRun")]
        public void TestTafEventsWork()
        {
            new TestsRunner(Assembly.GetExecutingAssembly(), false)
                .RunTests();

            Assert.That(USuiteForTafEvents.Output, Is.EqualTo(_expectedOutput));
        }

        private static void SuiteStartEvent(TestSuite suite) =>
            USuiteForTafEvents.Output.Add("OnSuiteStart");

        private static void SuiteFinishEvent(TestSuite suite) =>
            USuiteForTafEvents.Output.Add("OnSuiteFinish");

        private static void SuiteSkipEvent(TestSuite suite) =>
            USuiteForTafEvents.Output.Add("OnSuiteSkip");

        private static void SuiteMethodStartEvent(SuiteMethod suiteMethod) =>
            USuiteForTafEvents.Output.Add("OnSuiteMethodStart");

        private static void SuiteMethodFinishEvent(SuiteMethod suiteMethod) =>
            USuiteForTafEvents.Output.Add("OnSuiteMethodFinish");

        private static void SuiteMethodPassEvent(SuiteMethod suiteMethod) =>
            USuiteForTafEvents.Output.Add("OnSuiteMethodPass");

        private static void SuiteMethodFailEvent(SuiteMethod suiteMethod) =>
            USuiteForTafEvents.Output.Add("OnSuiteMethodFail");

        private static void TestStartEvent(Test test) =>
            USuiteForTafEvents.Output.Add("OnTestStart");

        private static void TestFinishEvent(Test test) =>
            USuiteForTafEvents.Output.Add("OnTestFinish");

        private static void TestPassEvent(Test test) =>
            USuiteForTafEvents.Output.Add("OnTestPass");

        private static void TestFailEvent(Test test) =>
            USuiteForTafEvents.Output.Add("OnTestFail");

        private static void TestSkipEvent(Test test) =>
            USuiteForTafEvents.Output.Add("OnTestSkip");

        private static List<string> GetRunAllExpecteds() =>
            new List<string>
            {
                "OnSuiteStart",
                "OnSuiteMethodStart",
                "BeforeSuite",
                "OnSuiteMethodPass",
                "OnSuiteMethodFinish",
                "OnSuiteMethodStart",
                "BeforeTest",
                "OnSuiteMethodPass",
                "OnSuiteMethodFinish",
                "OnTestStart",
                "Test1",
                "OnTestPass",
                "OnTestFinish",
                "OnSuiteMethodStart",
                "AfterTest",
                "OnSuiteMethodPass",
                "OnSuiteMethodFinish",
                "OnSuiteMethodStart",
                "BeforeTest",
                "OnSuiteMethodPass",
                "OnSuiteMethodFinish",
                "OnTestStart",
                "OnTestFail",
                "OnTestFinish",
                "OnSuiteMethodStart",
                "AfterTest",
                "OnSuiteMethodPass",
                "OnSuiteMethodFinish",
                "OnSuiteMethodStart",
                "AfterSuite",
                "OnSuiteMethodPass",
                "OnSuiteMethodFinish",
                "OnSuiteFinish"
            };

        private static List<string> GetFailBeforeSuiteExpecteds() =>
            new List<string>
            {
                "OnSuiteStart",
                "OnSuiteMethodStart",
                "OnSuiteMethodFail",
                "OnSuiteMethodFinish",
                "OnTestSkip",
                "OnTestSkip",
                "OnSuiteSkip",
                "OnSuiteMethodStart",
                "AfterSuite",
                "OnSuiteMethodPass",
                "OnSuiteMethodFinish",
                "OnSuiteFinish"
            };

        private static List<string> GetFailBeforeTestExpecteds() =>
            new List<string>
            {
                "OnSuiteStart",
                "OnSuiteMethodStart",
                "BeforeSuite",
                "OnSuiteMethodPass",
                "OnSuiteMethodFinish",
                "OnSuiteMethodStart",
                "OnSuiteMethodFail",
                "OnSuiteMethodFinish",
                "OnTestSkip",
                "OnSuiteMethodStart",
                "OnSuiteMethodFail",
                "OnSuiteMethodFinish",
                "OnTestSkip",
                "OnSuiteMethodStart",
                "AfterSuite",
                "OnSuiteMethodPass",
                "OnSuiteMethodFinish",
                "OnSuiteFinish"
            };
    }
}
