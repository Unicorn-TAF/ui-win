using System.Collections.Generic;
using Unicorn.Core.Testing.Tests;

namespace Unicorn.ReportPortalAgent
{
    public partial class ReportPortalListener
    {
        private readonly Dictionary<Core.Logging.LogLevel, ReportPortal.Client.Models.LogLevel> logLevels =
            new Dictionary<Core.Logging.LogLevel, ReportPortal.Client.Models.LogLevel>
        {
            { Core.Logging.LogLevel.Error, ReportPortal.Client.Models.LogLevel.Error },
            { Core.Logging.LogLevel.Warning, ReportPortal.Client.Models.LogLevel.Warning },
            { Core.Logging.LogLevel.Info, ReportPortal.Client.Models.LogLevel.Info },
            { Core.Logging.LogLevel.Debug, ReportPortal.Client.Models.LogLevel.Debug },
            { Core.Logging.LogLevel.Trace, ReportPortal.Client.Models.LogLevel.Trace },
        };

        private SuiteMethod currentTest = null;

        internal void ReportTestMessage(Core.Logging.LogLevel level, string info)
        {
            if (this.currentTest != null)
            {
                this.AddLog(this.currentTest.Id, logLevels[level], info);
            }
        }
    }
}
