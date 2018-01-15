using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private static TimeSpan testTimeout = TimeSpan.FromMinutes(15);
        private static TimeSpan suiteTimeout = TimeSpan.FromMinutes(60);
        private static Parallelization parallelBy = Parallelization.Assembly;
        private static int threads = 1;
        private static List<string> categories = new List<string>();
        private static List<string> features = new List<string>();
        private static List<string> tests = new List<string>();

        public static TimeSpan TestTimeout
        {
            get
            {
                return testTimeout;
            }

            set
            {
                testTimeout = value;
            }
        }

        public static TimeSpan SuiteTimeout
        {
            get
            {
                return suiteTimeout;
            }

            set
            {
                suiteTimeout = value;
            }
        }

        public static Parallelization ParallelBy
        {
            get
            {
                return parallelBy;
            }

            set
            {
                parallelBy = value;
            }
        }

        public static int Threads
        {
            get
            {
                 return threads;
            }

            set
            {
                threads = value;
            }
        }

        public static List<string> RunCategories => categories;

        public static List<string> RunFeatures => features;

        public static List<string> RunTests { get; }

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
        /// <para>If path is not specified 'unicorn.conf' from current directory is used</para>
        /// </summary>
        /// <param name="configPath">path to JSON config file </param>
        public static void FillFromFile(string configPath = "")
        {
            if (string.IsNullOrEmpty(configPath))
            {
                configPath = Path.GetDirectoryName(new Uri(typeof(Configuration).Assembly.CodeBase).LocalPath) + "/unicorn.conf";
            }

            JsonConf conf = JsonConvert.DeserializeObject<JsonConf>(File.ReadAllText(configPath));

            testTimeout = conf.TestTimeout;
            suiteTimeout = conf.SuiteTimeout;
            parallelBy = conf.ParallelBy;
            threads = conf.Threads;
            SetTestCategories(conf.RunCategories.ToArray());
            SetSuiteFeatures(conf.RunFeatures.ToArray());
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
            public List<string> RunTests { get; set; }
        }
    }
}
