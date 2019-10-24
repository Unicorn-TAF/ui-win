using System;
using System.Reflection;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Steps;
using Unicorn.Taf.Core.Testing;

namespace Unicorn.ReportPortalAgent
{
    /// <summary>
    /// Report portal reporter instance. Contains subscriptions to corresponding Unicorn events.
    /// </summary>
    public sealed class ReportPortalReporterInstance : IDisposable
    {
        private readonly ReportPortalListener listener;
        private readonly bool externalLaunch = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportPortalReporterInstance"/> class.
        /// </summary>
        public ReportPortalReporterInstance() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportPortalReporterInstance"/> class based on existing launch ID.
        /// If ID is null, then starts new launch on RP.
        /// </summary>
        /// <param name="existingLaunchId">existing launch ID</param>
        public ReportPortalReporterInstance(string existingLaunchId)
        {
            if (ReportPortalListener.Config.IsEnabled)
            {
                this.listener = new ReportPortalListener();

                if (!string.IsNullOrEmpty(existingLaunchId))
                {
                    externalLaunch = true;
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

        /// <summary>
        /// Reports logs to current executing suite method
        /// </summary>
        /// <param name="method">method itself</param>
        /// <param name="arguments">method arguments</param>
        public void ReportInfo(MethodBase method, object[] arguments) =>
            this.listener.ReportTestMessage(
                LogLevel.Info, 
                StepsUtilities.GetStepInfo(method, arguments));

        /// <summary>
        /// Sets list of tags which are common for all suites and specific for the run
        /// </summary>
        /// <param name="tags">list of tags</param>
        public void SetCommonSuitesTags(params string[] tags) =>
            this.listener.SetCommonSuitesTags(tags);

        /// <summary>
        /// Unsubscribe from events and finish launch if it is not external
        /// </summary>
        public void Dispose()
        {
            if (ReportPortalListener.Config.IsEnabled)
            {
                if (!externalLaunch)
                {
                    this.listener.FinishRun();
                }

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
