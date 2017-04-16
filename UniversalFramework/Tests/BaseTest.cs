using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    class BaseTest
    {
        [TearDown]
        public void TearDown()
        {
            //if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
                
        }
    }
}
