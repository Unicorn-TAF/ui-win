using System;
using System.Reflection;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Steps;
using Unicorn.Taf.Core.Testing;

namespace Unicorn.ReportPortalAgent
{
    public sealed class ReportPortalReporterInstance : IDisposable
    {
        private readonly ReportPortalListener listener;

        public ReportPortalReporterInstance() : this(null)
        {
        }

        public ReportPortalReporterInstance(string existingLaunchId)
        {
            if (ReportPortalListener.Config.IsEnabled)
            {
                this.listener = new ReportPortalListener();

                if (!string.IsNullOrEmpty(existingLaunchId))
                {
                    this.listener.ExistingLaunchId = existingLaunchId;
                }

                this.listener.StartRun();

                Test.OnTestStart += ReportTestStart;
                Test.OnTestFinish += ReportTestFinish;
                Test.OnTestSkip += this.listener.ReportTestSkipped;

                SuiteMethod.OnSuiteMethodStart += this.listener.StartSuiteMethod;
                SuiteMethod.OnSuiteMethodFinish += this.listener.FinishSuiteMethod;

                TestSuite.OnSuiteStart += this.ReportSuiteStart;
                TestSuite.OnSuiteFinish += this.ReportSuiteFinish;

                StepsEvents.OnStepStart += ReportInfo;
            }
        }

        public void ReportInfo(MethodBase method, object[] arguments)
        {
            string info = StepsUtilities.GetStepInfo(method, arguments);
            this.ReportInfo(info);
            Logger.Instance.Log(LogLevel.Info, "STEP: " + info);
        }

        public void ReportInfo(string info) =>
            this.listener.ReportTestMessage(LogLevel.Info, info);

        public void ReportSuiteFinish(TestSuite testSuite) =>
            this.listener.FinishSuite(testSuite);

        public void ReportSuiteStart(TestSuite testSuite) =>
            this.listener.StartSuite(testSuite);

        public void ReportTestFinish(Test test) =>
            this.listener.FinishSuiteMethod(test);

        public void ReportTestStart(Test test) =>
            this.listener.StartSuiteMethod(test);

        public void Dispose()
        {
            if (ReportPortalListener.Config.IsEnabled)
            {
                this.listener.FinishRun();

                Test.OnTestStart -= ReportTestStart;
                Test.OnTestFinish -= ReportTestFinish;
                Test.OnTestSkip -= this.listener.ReportTestSkipped;

                SuiteMethod.OnSuiteMethodStart -= this.listener.StartSuiteMethod;
                SuiteMethod.OnSuiteMethodFinish -= this.listener.FinishSuiteMethod;

                TestSuite.OnSuiteStart -= this.ReportSuiteStart;
                TestSuite.OnSuiteFinish -= this.ReportSuiteFinish;

                StepsEvents.OnStepStart -= ReportInfo;
            }
        }
    }
}
