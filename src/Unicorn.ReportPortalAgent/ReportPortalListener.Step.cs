using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        private readonly ConcurrentDictionary<Guid, SuiteMethod> _currentTests = new ConcurrentDictionary<Guid, SuiteMethod>();

        internal void ReportTestMessage(Taf.Core.Logging.LogLevel level, string info)
        {
            var stackTrace = new StackTrace();
            var currentTest = _currentTests.Values.First(t => stackTrace.GetFrames().Any(sf => sf.GetMethod().Name.Contains(t.TestMethod.Name)));

            if (currentTest != null)
            {
                AddLog(currentTest.Outcome.Id, _logLevels[level], info);
            }
        }
    }
}
