using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using ReportPortal.Client;
using ReportPortal.Client.Models;
using ReportPortal.Shared;
using ReportPortal.UnicornExtension.Configuration;
using Unicorn.Core.Testing.Tests;

namespace ReportPortal.UnicornExtension
{
    public partial class ReportPortalListener
    {
        private static Dictionary<Result, Status> statusMap = new Dictionary<Result, Status>();

        private Dictionary<Guid, TestReporter> suitesFlow = new Dictionary<Guid, TestReporter>();
        private Dictionary<Guid, TestReporter> testFlowIds = new Dictionary<Guid, TestReporter>();
        private Dictionary<string, TestReporter> testFlowNames = new Dictionary<string, TestReporter>();

        static ReportPortalListener()
        {
            var configPath = Path.GetDirectoryName(new Uri(typeof(Config).Assembly.CodeBase).LocalPath) + "/ReportPortal.conf";
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

            statusMap[Result.PASSED] = Status.Passed;
            statusMap[Result.FAILED] = Status.Failed;
            statusMap[Result.SKIPPED] = Status.Skipped;
            statusMap[Result.NOT_EXECUTED] = Status.None;
        }

        public static Config Config
        {
            get;

            private set;
        }

        public string ExistingLaunchId
        {
            get;

            set;
        }

        public void ReportRunStarted()
        {
            if (Config.IsEnabled)
            {
                StartRun();
            }
        }

        public void ReportRunFinished()
        {
            if (Config.IsEnabled)
            {
                FinishRun();
            }
        }

        public void ReportSuiteStarted(TestSuite suite)
        {
            if (Config.IsEnabled)
            {
                StartSuite(suite);
            }
        }

        public void ReportSuiteFinished(TestSuite suite)
        {
            if (Config.IsEnabled)
            {
                FinishSuite(suite);
            }
        }

        public void ReportSuiteMethodStarted(TestSuiteMethod test)
        {
            if (Config.IsEnabled)
            {
                StartSuiteMethod(test);
            }
        }

        public void ReportSuiteMethodFinished(TestSuiteMethod test)
        {
            if (Config.IsEnabled)
            {
                FinishSuiteMethod(test);
            }
        }

        public void ReportTestStarted(Test test)
        {
            if (Config.IsEnabled)
            {
                StartTest(test);
            }
        }

        public void ReportTestFinished(Test test)
        {
            if (Config.IsEnabled)
            {
                FinishTest(test);
            }
        }

        public void ReportTestSkipped(Test test)
        {
            if (Config.IsEnabled)
            {
                StartTest(test);
                FinishTest(test);
            }
        }

        public void ReportTestOutput(string report)
        {
            if (Config.IsEnabled)
            {
                TestOutput(report);
            }
        }

        public void ReportAddAttachment(Test test, string name, string mime, byte[] content)
        {
            if (Config.IsEnabled)
            {
                AddAttachment(test, name, mime, content);
            }
        }

        public void ReportAddTestTags(Test test, params string[] tags)
        {
            if (Config.IsEnabled)
            {
                AddTestTags(test, tags);
            }
        }

        public void ReportAddSuiteTags(TestSuite suite, params string[] tags)
        {
            if (Config.IsEnabled)
            {
                AddSuiteTags(suite, tags);
            }
        }

        public void ReportMergeLaunches(string descriptionSearchString)
        {
            if (Config.IsEnabled)
            {
                MergeRuns(descriptionSearchString);
            }
        }

        public string ReportGetLaunchId(string descriptionSearchString)
        {
            if (Config.IsEnabled)
            {
                return GetLaunchId(descriptionSearchString);
            }
            else
            {
                return null;
            }
        }
    }
}
