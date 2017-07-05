using NUnit.Framework;
using ProjectSpecific.Util;
using Unicorn.Core.Logging;
using Unicorn.Core.Reporting;

namespace ProjectSpecific
{
    [TestFixture]
    public class NUnitTestRunner
    {
        
        

        [OneTimeSetUp]
        public static void ClassInit()
        {
            Logger.Instance = new ConsoleLogger();
            Reporter.Instance = new ExcelReporter();
        }



        [OneTimeTearDown]
        public static void ClassTearDown()
        {

        }
    }
}
