using Newtonsoft.Json;
using ReportPortal.Client;
using ReportPortal.Client.Models;
using ReportPortal.UnicornExtension.Configuration;
using ReportPortal.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Unicorn.Core.Testing.Tests;

namespace ReportPortal.UnicornExtension
{
    public partial class ReportPortalListener
    {
        static ReportPortalListener()
        {
            var configPath = Path.GetDirectoryName(new Uri(typeof(Config).Assembly.CodeBase).LocalPath) + "/ReportPortal.conf";
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configPath));

            Service rpService;
            if (Config.Server.Proxy != null)
            {
                rpService = new Service(Config.Server.Url, Config.Server.Project, Config.Server.Authentication.Uuid, new WebProxy(Config.Server.Proxy));
            }
            else
            {
                rpService = new Service(Config.Server.Url, Config.Server.Project, Config.Server.Authentication.Uuid);
            }

            Bridge.Service = rpService;

            _statusMap[Result.PASSED] = Status.Passed;
            _statusMap[Result.FAILED] = Status.Failed;
            _statusMap[Result.SKIPPED] = Status.Skipped;
            _statusMap[Result.NOT_EXECUTED] = Status.None;
        }

        private static Dictionary<Result, Status> _statusMap = new Dictionary<Result, Status>();

        private Dictionary<Guid, TestReporter> _suitesFlow = new Dictionary<Guid, TestReporter>();
        private Dictionary<Guid, TestReporter> _testFlowIds = new Dictionary<Guid, TestReporter>();
        private Dictionary<string, TestReporter> _testFlowNames = new Dictionary<string, TestReporter>();

        public static Config Config { get; private set; }


        public void ReportRunStarted()
        {
            if (Config.IsEnabled)
                StartRun();
        }


        public void ReportRunFinished()
        {
            if (Config.IsEnabled)
                FinishRun();
        }


        public void ReportSuiteStarted(TestSuite suite)
        {
            if (Config.IsEnabled)
                StartSuite(suite);
        }


        public void ReportSuiteFinished(TestSuite suite)
        {
            if (Config.IsEnabled)
                FinishSuite(suite);
        }


        public void ReportTestStarted(Test test)
        {
            if (Config.IsEnabled)
                StartTest(test);
        }


        public void ReportTestFinished(Test test)
        {
            if (Config.IsEnabled)
                FinishTest(test);
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
                TestOutput(report);
        }


        public void ReportAddAttachment(Test test, string name, string mime, byte[] content)
        {
            if (Config.IsEnabled)
                AddAttachment(test, name, mime, content);
        }


        public void ReportAddTestTags(Test test, params string[] tags)
        {
            if (Config.IsEnabled)
                AddTestTags(test, tags);
        }


        public void ReportAddSuiteTags(TestSuite suite, params string[] tags)
        {
            if (Config.IsEnabled)
                AddSuiteTags(suite, tags);
        }
        

        public void ReportMergeLaunches(string descriptionSearchString, string description)
        {
            if (Config.IsEnabled)
                MergeRuns(descriptionSearchString, description);
        }


    }
}
