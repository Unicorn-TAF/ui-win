using System.Collections.Generic;
using Unicorn.Taf.Core.Testing;

namespace Unicorn.ReportPortalAgent
{
    /// <summary>
    /// Report portal listener, which handles reporting stuff for all test items.
    /// </summary>
    public partial class ReportPortalListener
    {
        private readonly Dictionary<Taf.Core.Logging.LogLevel, ReportPortal.Client.Models.LogLevel> _logLevels =
            new Dictionary<Taf.Core.Logging.LogLevel, ReportPortal.Client.Models.LogLevel>
        {
            { Taf.Core.Logging.LogLevel.Error, ReportPortal.Client.Models.LogLevel.Error },
            { Taf.Core.Logging.LogLevel.Warning, ReportPortal.Client.Models.LogLevel.Warning },
            { Taf.Core.Logging.LogLevel.Info, ReportPortal.Client.Models.LogLevel.Info },
            { Taf.Core.Logging.LogLevel.Debug, ReportPortal.Client.Models.LogLevel.Debug },
            { Taf.Core.Logging.LogLevel.Trace, ReportPortal.Client.Models.LogLevel.Trace },
        };

        private SuiteMethod _currentTest = null;

        internal void ReportTestMessage(Taf.Core.Logging.LogLevel level, string info)
        {
            if (_currentTest != null)
            {
                AddLog(_currentTest.Outcome.Id, _logLevels[level], info);
            }
        }
    }
}
