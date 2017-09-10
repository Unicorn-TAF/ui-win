using Unicorn.Core.Logging;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Attributes;


namespace Tests.TestData
{
    class BaseTestSuite : TestSuite
    {

        [BeforeSuite]
        public void ClassInit()
        {
            Logger.Instance.Info("Before suite");
        }



        [AfterSuite]
        public void ClassTearDown()
        {
            Logger.Instance.Info("After suite");
        }
    }
}
