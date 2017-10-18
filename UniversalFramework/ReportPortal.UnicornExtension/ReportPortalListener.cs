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

            _statusMap[Result.PASSED.ToString()] = Status.Passed;
            _statusMap[Result.FAILED.ToString()] = Status.Failed;
            _statusMap[Result.SKIPPED.ToString()] = Status.Skipped;
        }

        private static Dictionary<string, Status> _statusMap = new Dictionary<string, Status>();

        private Dictionary<string, TestReporter> _suitesFlow = new Dictionary<string, TestReporter>();
        private Dictionary<string, TestReporter> _testFlowIds = new Dictionary<string, TestReporter>();
        private Dictionary<string, TestReporter> _testFlowNames = new Dictionary<string, TestReporter>();

        public static Config Config { get; private set; }


        public void CallerRunStarted()
        {
            if (Config.IsEnabled)
                StartRun();
        }


        public void CallerRunFinished()
        {
            if (Config.IsEnabled)
                FinishRun();
        }


        public void CallerSuiteStarted(TestSuite suite)
        {
            if (Config.IsEnabled)
                StartSuite(suite);
        }


        public void CallerSuiteFinished(TestSuite suite)
        {
            if (Config.IsEnabled)
                FinishSuite(suite);
        }


        public void CallerTestStarted(Test test)
        {
            if (Config.IsEnabled)
                StartTest(test);
        }


        public void CallerTestFinished(Test test)
        {
            if (Config.IsEnabled)
                FinishTest(test);
        }

        public void CallerTestSkipped(Test test)
        {
            if (Config.IsEnabled)
            {
                StartTest(test);
                FinishTest(test);
            }
        }

        public void CallerTestOutput(string report)
        {
            if (Config.IsEnabled)
                TestOutput(report);
        }
    }
}
