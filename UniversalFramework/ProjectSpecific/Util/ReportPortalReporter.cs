using ReportPortal.UnicornExtension;
using System;
using System.IO;
using System.Reflection;
using Unicorn.Core.Logging;
using Unicorn.Core.Reporting;
using Unicorn.Core.Testing.Steps;
using Unicorn.Core.Testing.Steps.Attributes;
using Unicorn.Core.Testing.Tests;

namespace ProjectSpecific.Util
{
    class ReportPortalReporter : IReporter
    {
        ReportPortalListener Listener;
        public void Complete()
        {
            Listener.ReportRunFinished();
        }

        public void Init()
        {
            string screenshotsDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Screenshots");
            if (!Directory.Exists(screenshotsDir))
                Directory.CreateDirectory(screenshotsDir);

            Listener = new ReportPortalListener();

            Listener.ReportRunStarted();

            Test.onStart += this.ReportTestStart;
            Test.onFail += this.TakeScreenshot;
            Test.onFinish += this.ReportTestFinish;
            Test.onSkip += this.ReportTestSkip;

            TestSuite.onStart += this.ReportSuiteStart;
            TestSuite.onFinish += this.ReportSuiteFinish;

            TestStepsEvents.onStart += ReportInfo;
        }

        public void ReportInfo(MethodBase method, object[] arguments)
        {
            string stepDescription = string.Empty;

            object[] attributes = method.GetCustomAttributes(typeof(TestStep), true);

            //ParameterInfo[] methodParams = method.GetParameters();

            //string[] parametersStr = new string[methodParams.Length];
            //for (int i = 0; i < methodParams.Length; i++)
            //    parametersStr[i] = methodParams[i].ToString();

            if (attributes.Length == 0)
            {
                stepDescription = method.Name;

                if (arguments.Length > 0)
                    stepDescription += ":";
                for (int i = 0; i < arguments.Length; i++)
                    stepDescription += $" '{arguments[i]}'";
            }
            else
            {
                TestStep attribute = (TestStep)attributes[0];
                stepDescription = attribute.Description;
                stepDescription = string.Format(stepDescription, arguments);
            }

            string info = "STEP: " + stepDescription;
            Logger.Instance.Info(info);
            Listener.ReportTestOutput(info);
        }

        public void ReportInfo(string info)
        {
            Listener.ReportTestOutput(info);
        }

        public void ReportSuiteFinish(TestSuite testSuite)
        {
            Listener.ReportSuiteFinished(testSuite);
            Listener.ReportAddSuiteTags(testSuite, Environment.MachineName);
        }

        public void ReportSuiteStart(TestSuite testSuite)
        {
            Listener.ReportSuiteStarted(testSuite);
        }

        public void ReportTestFinish(Test test)
        {
            Listener.ReportTestFinished(test);
            Listener.ReportAddTestTags(test, Environment.MachineName);
        }

        public void ReportTestStart(Test test)
        {
            Listener.ReportTestStarted(test);
            
        }

        public void ReportTestSkip(Test test)
        {
            Listener.ReportTestSkipped(test);
        }


        private void TakeScreenshot(Test test)
        {
            string screenshotName = test.FullTestName;
            if (screenshotName.Length > 150)
                screenshotName = screenshotName.Substring(0, 150) + "~";

            Screenshot.TakeScreenshot(screenshotName);
            test.Outcome.Screenshot = screenshotName + ".Jpeg";
        }
    }
}
