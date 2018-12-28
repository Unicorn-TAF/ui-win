using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Unicorn.Core.Testing.Tests.Adapter
{
    public enum Parallelization
    {
        Assembly,
        Suite,
        Test
    }

    public static class Configuration
    {
        private static List<string> categories = new List<string>();
        private static List<string> features = new List<string>();
        private static List<string> tests = new List<string>();

        public static TimeSpan TestTimeout { get; set; } = TimeSpan.FromMinutes(15);

        public static TimeSpan SuiteTimeout { get; set; } = TimeSpan.FromMinutes(60);

        public static Parallelization ParallelBy { get; set; } = Parallelization.Assembly;

        public static int Threads { get; set; } = 1;

        public static List<string> RunCategories => categories;

        public static List<string> RunFeatures => features;

        public static List<string> RunTests => tests;

        /// <summary>
        /// Set tests categories needed to be run.
        /// All categories are converted in upper case. Blank categories are ignored
        /// </summary>
        /// <param name="categoriesToRun">array of categories</param>
        public static void SetTestCategories(params string[] categoriesToRun) =>
            categories = categoriesToRun
                .Select(v => { return v.ToUpper().Trim().Replace(".", @"\.").Replace("*", "[A-z0-9]*").Replace("~", ".*"); })
                .Where(v => !string.IsNullOrEmpty(v))
                .ToList();

        /// <summary>
        /// Set features on which test suites needed to be run.
        /// All features are converted in upper case. Blank features are ignored
        /// </summary>
        /// <param name="featuresToRun">array of features</param>
        public static void SetSuiteFeatures(params string[] featuresToRun) =>
            features = featuresToRun
                .Select(v => { return v.ToUpper().Trim(); })
                .Where(v => !string.IsNullOrEmpty(v))
                .ToList();

        /// <summary>
        /// Set features on which test suites needed to be run.
        /// All features are converted in upper case. Blank features are ignored
        /// </summary>
        /// <param name="featuresToRun">array of features</param>
        public static void SetTestsMasks(params string[] testsToRun) =>
            tests = testsToRun
                .Select(v => { return v.Trim(); })
                .Where(v => !string.IsNullOrEmpty(v))
                .ToList();

        /// <summary>
        /// Deserialize run configuration fro JSON file
        /// </summary>
        /// <param name="configPath">path to JSON config file </param>
        public static void FillFromFile(string configPath = "")
        {
            if (string.IsNullOrEmpty(configPath))
            {
                configPath = Path.GetDirectoryName(new Uri(typeof(Configuration).Assembly.CodeBase).LocalPath) + "/unicorn.conf";
            }

            JsonConf conf = JsonConvert.DeserializeObject<JsonConf>(File.ReadAllText(configPath));

            TestTimeout = conf.JsonTestTimeout;
            SuiteTimeout = conf.JsonSuiteTimeout;
            ParallelBy = conf.JsonParallelBy;
            Threads = conf.JsonThreads;
            SetTestCategories(conf.JsonRunCategories.ToArray());
            SetSuiteFeatures(conf.JsonRunFeatures.ToArray());
            SetTestsMasks(conf.JsonRunTests.ToArray());
        }

        public static string GetInfo()
        {
            StringBuilder info = new StringBuilder();

            info.AppendLine($"Features to run: {string.Join(",", RunFeatures)}")
                .AppendLine($"Categories to run: {string.Join(",", RunCategories)}")
                .AppendLine($"Tests filter: {string.Join(",", RunTests)}")
                .AppendLine($"Parallel by '{ParallelBy}' to '{Threads}' thread(s)")
                .AppendLine($"Test run timeout: {TestTimeout}")
                .AppendLine($"Suite run timeout: {SuiteTimeout}");

            return info.ToString();
        }

        internal class JsonConf
        {
            [JsonProperty("testTimeout")]
            private int testTimeout = 15;

            [JsonProperty("suiteTimeout")]
            private int suiteTimeout = 60;

            [JsonProperty("parallel")]
            private string parallelBy = "assembly";

            [JsonIgnore]
            public TimeSpan JsonTestTimeout => TimeSpan.FromMinutes(this.testTimeout);

            [JsonIgnore]
            public TimeSpan JsonSuiteTimeout => TimeSpan.FromMinutes(this.suiteTimeout);

            [JsonIgnore]
            public Parallelization JsonParallelBy
            {
                get
                {
                    switch (this.parallelBy.ToLower())
                    {
                        case "suite":
                            return Parallelization.Suite;
                        case "test":
                            return Parallelization.Test;
                        case "assembly":
                        default:
                            return Parallelization.Assembly;
                    }
                }
            }

            [JsonProperty("threads")]
            public int JsonThreads { get; set; } = 1;

            [JsonProperty("categories")]
            public List<string> JsonRunCategories { get; set; } = new List<string>();

            [JsonProperty("features")]
            public List<string> JsonRunFeatures { get; set; } = new List<string>();

            [JsonProperty("tests")]
            public List<string> JsonRunTests { get; set; } = new List<string>();
        }
    }
}
