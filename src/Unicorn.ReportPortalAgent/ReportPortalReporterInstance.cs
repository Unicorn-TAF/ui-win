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

                Test.OnTestStart += this.listener.StartSuiteMethod;
                Test.OnTestFinish += this.listener.FinishSuiteMethod;
                Test.OnTestSkip += this.listener.SkipSuiteMethod;

                SuiteMethod.OnSuiteMethodStart += this.listener.StartSuiteMethod;
                SuiteMethod.OnSuiteMethodFinish += this.listener.FinishSuiteMethod;

                TestSuite.OnSuiteStart += this.listener.StartSuite;
                TestSuite.OnSuiteFinish += this.listener.FinishSuite;

                StepsEvents.OnStepStart += ReportInfo;
            }
        }

        public void ReportInfo(MethodBase method, object[] arguments) =>
            this.listener.ReportTestMessage(
                LogLevel.Info, 
                StepsUtilities.GetStepInfo(method, arguments));

        public void Dispose()
        {
            if (ReportPortalListener.Config.IsEnabled)
            {
                this.listener.FinishRun();

                Test.OnTestStart -= this.listener.StartSuiteMethod;
                Test.OnTestFinish -= this.listener.FinishSuiteMethod;
                Test.OnTestSkip -= this.listener.SkipSuiteMethod;

                SuiteMethod.OnSuiteMethodStart -= this.listener.StartSuiteMethod;
                SuiteMethod.OnSuiteMethodFinish -= this.listener.FinishSuiteMethod;

                TestSuite.OnSuiteStart -= this.listener.StartSuite;
                TestSuite.OnSuiteFinish -= this.listener.FinishSuite;

                StepsEvents.OnStepStart -= ReportInfo;
            }
        }
    }
}
