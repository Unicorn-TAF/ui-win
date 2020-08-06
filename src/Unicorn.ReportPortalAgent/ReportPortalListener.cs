using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using ReportPortal.Client;
using ReportPortal.Shared;
using ReportPortal.Shared.Reporter;
using Unicorn.ReportPortalAgent.Configuration;
using Unicorn.Taf.Core.Testing;

namespace Unicorn.ReportPortalAgent
{
    /// <summary>
    /// Report portal listener, which handles reporting stuff for all test items.
    /// </summary>
    public partial class ReportPortalListener
    {
        private static readonly Dictionary<Status, ReportPortal.Client.Models.Status> _statusMap = new Dictionary<Status, ReportPortal.Client.Models.Status>();

        private readonly Dictionary<Guid, ITestReporter> _suitesFlow = new Dictionary<Guid, ITestReporter>();
        private readonly Dictionary<Guid, ITestReporter> _testFlowIds = new Dictionary<Guid, ITestReporter>();
        private string[] _commonSuitesTags = null;

        static ReportPortalListener()
        {
            var configPath = Path.Combine(
                Path.GetDirectoryName(new Uri(typeof(Config).Assembly.CodeBase).LocalPath),
                "ReportPortal.conf");
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configPath));

            Service reportPortalService;
            if (Config.Server.Proxy != null)
            {
                reportPortalService = new Service(Config.Server.Url, Config.Server.Project, Config.Server.Authentication.Uuid, new WebProxy(Config.Server.Proxy));
            }
            else
            {
                reportPortalService = new Service(Config.Server.Url, Config.Server.Project, Config.Server.Authentication.Uuid);
            }

            Bridge.Service = reportPortalService;

            _statusMap[Status.Passed] = ReportPortal.Client.Models.Status.Passed;
            _statusMap[Status.Failed] = ReportPortal.Client.Models.Status.Failed;
            _statusMap[Status.Skipped] = ReportPortal.Client.Models.Status.Skipped;
        }

        /// <summary>
        /// Gets Report Portal configuration
        /// </summary>
        public static Config Config
        {
            get;

            private set;
        }

        /// <summary>
        /// Gets or sets id of existing Report Portal launch
        /// </summary>
        public string ExistingLaunchId
        {
            get;

            set;
        }

        /// <summary>
        /// Add attachment to test
        /// </summary>
        /// <param name="test"><see cref="Test"/> instance</param>
        /// <param name="text">attachment text</param>
        /// <param name="attachmentName">attachment name</param>
        /// <param name="mime">mime type</param>
        /// <param name="content">content in bytes</param>
        public void ReportAddAttachment(Test test, string text, string attachmentName, string mime, byte[] content)
        {
            if (Config.IsEnabled && _testFlowIds.ContainsKey(test.Outcome.Id))
            {
                AddAttachment(test.Outcome.Id, ReportPortal.Client.Models.LogLevel.Info, text, attachmentName, mime, content);
            }
        }

        /// <summary>
        /// Sets list of tags which are common for all suites and specific for the run
        /// </summary>
        /// <param name="tags">list of tags</param>
        public void SetCommonSuitesTags(params string[] tags) =>
            _commonSuitesTags = tags;
    }
}
