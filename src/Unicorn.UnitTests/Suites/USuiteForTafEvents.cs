using System.Collections.Generic;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.UnitTests.Suites
{
    public enum TafEventsSuiteMode
    {
        RunAll,
        FailBeforeSuite,
        FailBeforeTest
    }

    [Suite("Suite for TAF events")]
    [Tag(Tag.TafEvents)]
    public class USuiteForTafEvents : TestSuite
    {
        internal static TafEventsSuiteMode RunMode { get; set; } = TafEventsSuiteMode.RunAll;

        internal static List<string> Output { get; set; } = new List<string>();

        [BeforeSuite]
        public void BeforeSuite()
        {
            if (RunMode == TafEventsSuiteMode.FailBeforeSuite)
            {
                throw new TestTimeoutException();
            }
            
            Output.Add("BeforeSuite");
        }

        [BeforeTest]
        public void BeforeTest()
        {
            if (RunMode == TafEventsSuiteMode.FailBeforeTest)
            {
                throw new TestTimeoutException();
            }

            Output.Add("BeforeTest");
        }

        [Test]
        public void Test1() =>
            Output.Add("Test1");

        [Test]
        public void Test2() =>
            throw new TestTimeoutException();

        [AfterTest]
        public void AfterTest() =>
            Output.Add("AfterTest");

        [AfterSuite]
        public void AfterSuite() =>
            Output.Add("AfterSuite");
    }
}
