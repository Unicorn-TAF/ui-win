using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Text;

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
        public static void SetTestCategories(params string[] categoriesToRun)
        {
            categories = categoriesToRun
                .Select(v => { return v.ToUpper().Trim(); })
                .Where(v => !string.IsNullOrEmpty(v))
                .ToList();
        }

        /// <summary>
        /// Set features on which test suites needed to be run.
        /// All features are converted in upper case. Blank features are ignored
        /// </summary>
        /// <param name="featuresToRun">array of features</param>
        public static void SetSuiteFeatures(params string[] featuresToRun)
        {
            features = featuresToRun
                .Select(v => { return v.ToUpper().Trim(); })
                .Where(v => !string.IsNullOrEmpty(v))
                .ToList();
        }

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

            TestTimeout = conf.TestTimeout;
            SuiteTimeout = conf.SuiteTimeout;
            ParallelBy = conf.ParallelBy;
            Threads = conf.Threads;
            SetTestCategories(conf.RunCategories.ToArray());
            SetSuiteFeatures(conf.RunFeatures.ToArray());
        }

        public static string GetInfo()
        {
            StringBuilder info = new StringBuilder();

            info.AppendLine($"Features to run: {string.Join(",", RunFeatures)}")
                .AppendLine($"Categories to run: {string.Join(",", RunCategories)}")
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

            [JsonProperty("threads")]
            private int threads = 1;

            [JsonIgnore]
            private List<string> categories = new List<string>();
            [JsonIgnore]
            private List<string> features = new List<string>();
            [JsonIgnore]
            private List<string> tests = new List<string>();

            [JsonIgnore]
            public TimeSpan TestTimeout => TimeSpan.FromMinutes(this.testTimeout);

            [JsonIgnore]
            public TimeSpan SuiteTimeout => TimeSpan.FromMinutes(this.suiteTimeout);

            [JsonIgnore]
            public Parallelization ParallelBy
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

            [JsonIgnore]
            public int Threads => this.threads;

            [JsonProperty("categories")]
            public List<string> RunCategories => this.categories;

            [JsonProperty("features")]
            public List<string> RunFeatures => this.features;

            [JsonProperty("tests")]
            public List<string> RunTests => this.tests;
        }
    }
}
