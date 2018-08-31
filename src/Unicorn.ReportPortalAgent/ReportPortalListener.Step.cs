using System;
using ReportPortal.Client.Requests;
using Unicorn.Core.Testing.Tests;

namespace Unicorn.ReportPortalAgent
{
    public partial class ReportPortalListener
    {
        private SuiteMethod currentTest = null;

        protected void TestOutput(string info)
        {
            try
            {
                var fullTestName = this.currentTest.FullName;
                var message = info;

                if (this.testFlowNames.ContainsKey(fullTestName))
                {
                    this.testFlowNames[fullTestName].Log(new AddLogItemRequest { Level = ReportPortal.Client.Models.LogLevel.Info, Time = DateTime.UtcNow, Text = message });
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }

        protected void TestOutput(Core.Logging.LogLevel level, string info)
        {
            if (this.currentTest == null)
            {
                return;
            }

            try
            {
                var fullTestName = this.currentTest.FullName;
                var message = info;

                if (this.testFlowNames.ContainsKey(fullTestName))
                {
                    this.testFlowNames[fullTestName].Log(new AddLogItemRequest { Level = GetReportingLevel(level), Time = DateTime.UtcNow, Text = message });
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }

        private ReportPortal.Client.Models.LogLevel GetReportingLevel(Core.Logging.LogLevel level)
        {
            switch (level)
            {
                case Core.Logging.LogLevel.Trace:
                    return ReportPortal.Client.Models.LogLevel.Trace;
                case Core.Logging.LogLevel.Debug:
                    return ReportPortal.Client.Models.LogLevel.Debug;
                case Core.Logging.LogLevel.Error:
                    return ReportPortal.Client.Models.LogLevel.Error;
                case Core.Logging.LogLevel.Warning:
                    return ReportPortal.Client.Models.LogLevel.Warning;
                case Core.Logging.LogLevel.Info:
                    return ReportPortal.Client.Models.LogLevel.Info;
            }

            throw new NotSupportedException($"Log level {level} is not supported");
        }
    }
}
