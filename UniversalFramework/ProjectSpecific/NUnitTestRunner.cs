using NUnit.Framework;
using ProjectSpecific.Util;
using System.IO;
using System.Reflection;
using System.Text;
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
            Reporter.Instance.Init();
        }



        [OneTimeTearDown]
        public static void ClassTearDown()
        {

        }

        protected string GetTestContextOut()
        {
            TextWriter tout = TestContext.Out;
            TextWriter tout1 = (TextWriter)tout.GetType().GetField("_out", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(tout);
            StringBuilder sb = (StringBuilder)tout1.GetType().GetField("_sb", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(tout1);
            return sb.ToString();
        }
    }
}
