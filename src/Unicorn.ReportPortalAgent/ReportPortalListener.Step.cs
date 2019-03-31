using System.Collections.Generic;
using Unicorn.Taf.Core.Testing.Tests;

namespace Unicorn.ReportPortalAgent
{
    public partial class ReportPortalListener
    {
        private readonly Dictionary<Taf.Core.Logging.LogLevel, ReportPortal.Client.Models.LogLevel> logLevels =
            new Dictionary<Taf.Core.Logging.LogLevel, ReportPortal.Client.Models.LogLevel>
        {
            { Taf.Core.Logging.LogLevel.Error, ReportPortal.Client.Models.LogLevel.Error },
            { Taf.Core.Logging.LogLevel.Warning, ReportPortal.Client.Models.LogLevel.Warning },
            { Taf.Core.Logging.LogLevel.Info, ReportPortal.Client.Models.LogLevel.Info },
            { Taf.Core.Logging.LogLevel.Debug, ReportPortal.Client.Models.LogLevel.Debug },
            { Taf.Core.Logging.LogLevel.Trace, ReportPortal.Client.Models.LogLevel.Trace },
        };

        private SuiteMethod currentTest = null;

        internal void ReportTestMessage(Taf.Core.Logging.LogLevel level, string info)
        {
            if (this.currentTest != null)
            {
                this.AddLog(this.currentTest.Outcome.Id, logLevels[level], info);
            }
        }
    }
}
