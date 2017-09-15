using System;
using System.IO;
using System.Reflection;
using Unicorn.Core.Reporting;
using Unicorn.Core.Testing.Tests;

namespace ProjectSpecific.Util
{
    public class SimpleReporter : IReporter
    {
        public void Complete()
        {
            //throw new NotImplementedException();
        }

        public void Init()
        {
            string screenshotsDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Screenshots");

            if (!Directory.Exists(screenshotsDir))
                Directory.CreateDirectory(screenshotsDir);
            //throw new NotImplementedException();
        }

        public void ReportInfo(string info)
        {
            //throw new NotImplementedException();
        }

        public void ReportSuiteFinish(TestSuite testSuite)
        {
            //throw new NotImplementedException();
        }

        public void ReportTestStart(Test test)
        {
            //throw new NotImplementedException();
        }

        public void ReportTestFinish(Test test)
        {
            //throw new NotImplementedException();
        }
    }
}
